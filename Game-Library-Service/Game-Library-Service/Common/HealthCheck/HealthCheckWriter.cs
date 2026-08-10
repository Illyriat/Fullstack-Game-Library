using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Game_Library_Service.Common.HealthCheck
{
    /// <summary>
    /// Provides function compatible with the Health Check ResponseWriter delegate to format a custom response object.
    /// </summary>
    public static class HealthCheckWriter
    {
        /// <summary>
        /// Updates the Response for the given HttpContext object to return the expected response format.
        /// </summary>
        /// <param name="context">The HttpContext to update.</param>
        /// <param name="healthReport">The HealthReport to use as the source of the health check result.</param>
        /// <returns>Completed Task when HttpContext is updated.</returns>
        public static Task WriteResponse(HttpContext context, HealthReport healthReport)
        {
            var result = new HealthCheckResult
            {
                Status = healthReport.Status == HealthStatus.Healthy ? "ok" : healthReport.Status.ToString()
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }

    /// <summary>
    /// Returns an object representing the results of the health check call.
    /// </summary>
    public class HealthCheckResult
    {
        /// <summary>
        /// The Status - either "ok", "Unhealthy" or "Degraded"
        /// </summary>
        public string Status { get; init; } = string.Empty;

        /// <summary>
        /// The version of the API Assembly. Default "1.0.0".
        /// </summary>
        public string Version { get; init; } = "1.0.0";

        /// <summary>
        /// Default "dev".
        /// </summary>
        public string Commit { get; init; } = "dev";
    }
}
