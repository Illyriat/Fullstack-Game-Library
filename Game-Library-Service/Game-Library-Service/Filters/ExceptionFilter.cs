using FluentValidation;
using Game_Library_Service.Common.Dtos;
using Game_Library_Service.Common.Exceptions.CustomExceptions;
using Game_Library_Service.Common.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace Game_Library_Service.Filters
{
    /// <summary>
    /// Global exception filter for handling and logging unhandled exceptions in API requests.
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handles exceptions thrown during request processing and returns appropriate error responses.
        /// </summary>
        /// <param name="context">The exception context containing request and exception details.</param>
        public override async void OnException(ExceptionContext context)
        {
            var logger = (ILogger<ExceptionFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<ExceptionFilter>))!;
            var environment = (IWebHostEnvironment)context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment))!;
            try
            {
                var statusCode = HttpStatusCode.InternalServerError;
                var messageKey = "key_internalServerException";
                var errorMessage = context.Exception.Message.Replace("\"", "\\");

                if (context.Exception is ApiException apiException)
                {
                    statusCode = apiException.Code;
                    messageKey = apiException.MessageKey;
                }
                else if (context.Exception is ValidationException validationException)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Type = "ValidationFailure",
                        Title = "Validation error",
                        Detail = "One or more validation errors has occurred"
                    };
                    problemDetails.Extensions["errors"] = validationException.Errors.Select(x => x.ErrorMessage).ToList();

                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(problemDetails);
                    context.ExceptionHandled = true;
                    return;
                }
                else
                {
                    if (environment.IsProduction())
                    {
                        errorMessage = "Something went wrong. Please see the logs for more information.";
                    }

                    var logMessageBuilder = new StringBuilder();

                    logMessageBuilder.AppendLine($"{nameof(ExceptionFilter)} - Unhandled exception occurred.");

                    logMessageBuilder.AppendLine($"Endpoint URL: {context.HttpContext.Request.GetDisplayUrl()}");
                    logMessageBuilder.AppendLine();

                    var hasClientTypeHeader = context.HttpContext.Request.Headers.TryGetValue("X-Client-Type", out var clientType);
                    var clientTypeText = hasClientTypeHeader
                        ? clientType.ToString()
                        : "Not found.";

                    logMessageBuilder.AppendLine($"Client type: {clientTypeText}");
                    logMessageBuilder.AppendLine();

                    var hasRefererHeader = context.HttpContext.Request.Headers.TryGetValue("Referer", out var refererUrl);
                    var refererUrlText = hasRefererHeader
                        ? refererUrl.ToString()
                        : "Not found.";

                    logMessageBuilder.AppendLine($"Referer URL: {refererUrlText}");
                    logMessageBuilder.AppendLine();

                    var hasAuthHeader = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
                    var jwtClaimsText = "Not found.";

                    if (hasAuthHeader)
                    {
                        try
                        {
                            var jwtPayload = authorizationHeader.ToString().Split(".")[1];
                            jwtClaimsText = Base64UrlEncoder.Decode(jwtPayload);
                        }
                        catch (Exception ex)
                        {
                            logMessageBuilder.AppendLine("Error Decoding jwt: " + ex.ToString());
                        }
                    }

                    logMessageBuilder.AppendLine($"JWT claims: {jwtClaimsText}");
                    logMessageBuilder.AppendLine();

                    var requestBody = await context.HttpContext.Request.Body.ReadRequestBodyAsString();
                    if (string.IsNullOrEmpty(requestBody))
                    {
                        requestBody = "Not found.";
                    }

                    logMessageBuilder.AppendLine($"Request body: {requestBody}");
                    logMessageBuilder.AppendLine();

                    var innerExceptionText = context.Exception.InnerException != null
                        ? $"\n\nInner error: {context.Exception.InnerException}"
                        : string.Empty;

                    logMessageBuilder.Append($"Error: {context.Exception}{innerExceptionText}");

                    var errorLogMessage = logMessageBuilder.ToString();

                    logger.LogError("{errorLogMessage}", errorLogMessage);
                }

                var errorDto = new ErrorDto
                {
                    Error = errorMessage,
                    ErrorKey = messageKey.Replace("\"", "\\")
                };

                var result = new ObjectResult(errorDto)
                {
                    StatusCode = (int)statusCode,
                };

                result.ContentTypes.Add(new MediaTypeHeaderValue(MediaTypeNames.Application.Json));

                context.Result = result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Source :{Source},InnerException:{InnerException?.Message},Message: {Message}", ex.Source, ex.InnerException?.Message, ex.Message);
            }
        }
    }
}
