using Game_Library_Service.Common.OpenApi;
using Game_Library_Service.Data.Entities;
using Scalar.AspNetCore;

namespace Game_Library_Service.Common.Extensions.Startup
{
    /// <summary>
    /// WebApplication extensions for configuring Scalar and OpenAPI endpoints in development/local environments.
    /// </summary>
    public static class ScalarWebApplicationExtensions
    {
        private static readonly string[] ScalarDocuments = new[] { "v1", "entities" };

        /// <summary>
        /// Maps OpenAPI JSON endpoints, Scalar UI, and entity schema endpoints when running in Development or Local.
        /// </summary>
        /// <param name="app">The current <see cref="WebApplication"/> instance.</param>
        /// <returns>The same <see cref="WebApplication"/> instance for fluent chaining.</returns>
        public static WebApplication MapScalarForDev(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment() && app.Environment.EnvironmentName != "Local")
            {
                return app;
            }

            app.MapOpenApi("/openapi/{documentName}.json");

            app.MapScalarApiReference(options => options.AddDocuments(ScalarDocuments));

            app.MapGet("/", () => Results.Redirect("/scalar"));

            var schemaEndpoint = app.MapGet("/__schemas/entities", () => Results.NoContent())
                .WithName("Schemas_Entities")
                .WithMetadata(new EndpointGroupNameAttribute("entities"));

            ScalarOpenApiSetup.AddEntitySchemas(
                schemaEndpoint,
                typeof(AuditableEntity),
                typeof(Game),
                typeof(Publisher)
            );

            return app;
        }
    }
}
