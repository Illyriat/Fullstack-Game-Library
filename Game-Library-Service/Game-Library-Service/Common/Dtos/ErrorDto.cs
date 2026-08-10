namespace Game_Library_Service.Common.Dtos
{
    /// <summary>
    /// Error data transfer object for API responses.
    /// </summary>
    public record ErrorDto
    {
        /// <summary>
        /// Error message.
        /// </summary>
        public string Error { get; init; } = string.Empty;

        /// <summary>
        /// Localization key for the error message.
        /// </summary>
        public string ErrorKey { get; init; } = string.Empty;
    }
}
