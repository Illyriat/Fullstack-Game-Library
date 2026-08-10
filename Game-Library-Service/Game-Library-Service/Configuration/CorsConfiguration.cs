namespace Game_Library_Service.Configuration
{
    /// <summary>
    /// Represents strongly-typed configuration values for Cross-Origin Resource Sharing (CORS).
    /// </summary>
    public sealed class CorsConfiguration
    {
        /// <summary>
        /// The configuration section name used to bind CORS settings.
        /// </summary>
        public const string SectionName = "Cors";

        /// <summary>
        /// The name of the CORS policy to register and apply in the request pipeline.
        /// </summary>
        public string PolicyName { get; set; } = "AllowAll";

        /// <summary>
        /// When <see langword="true"/>, configures a permissive policy that allows any origin, method, and header.
        /// </summary>
        public bool AllowAll { get; set; } = true;

        /// <summary>
        /// Allowed origins to use when <see cref="AllowAll"/> is <see langword="false"/>.
        /// </summary>
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    }
}
