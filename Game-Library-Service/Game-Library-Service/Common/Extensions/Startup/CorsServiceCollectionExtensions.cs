using Game_Library_Service.Configuration;

namespace Game_Library_Service.Common.Extensions.Startup
{
    /// <summary>
    /// IServiceCollection extensions for configuring CORS.
    /// </summary>
    internal static class CorsServiceCollectionExtensions
    {
        /// <summary>
        /// Binds <see cref="CorsConfiguration"/> from configuration, registers it, and adds the configured CORS policy.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>The configured policy name to apply via <c>app.UseCors(name)</c>.</returns>
        internal static string ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsConfig = configuration.GetSection(CorsConfiguration.SectionName).Get<CorsConfiguration>()
                ?? throw new InvalidOperationException("CORS configuration section is missing or invalid.");

            services.AddSingleton(corsConfig);

            services.AddCors(options =>
            {
                options.AddPolicy(corsConfig.PolicyName, policy =>
                {
                    if (corsConfig.AllowAll)
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                    else
                    {
                        policy
                            .WithOrigins(corsConfig.AllowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }

                    policy.WithExposedHeaders("Content-Disposition");
                });
            });

            return corsConfig.PolicyName;
        }
    }
}
