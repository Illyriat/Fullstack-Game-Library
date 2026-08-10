using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Base class for API exceptions with HTTP status codes.
    /// </summary>
    [Serializable]
    public abstract class ApiException : Exception
    {
        /// <summary>
        /// Localization key for the exception message.
        /// </summary>
        public string MessageKey { get; private set; } = string.Empty;

        /// <summary>
        /// HTTP status code for the exception.
        /// </summary>
        public HttpStatusCode Code { get; private set; }

        /// <summary>
        /// Creates a new API exception with the specified status code, message, and localization key.
        /// </summary>
        /// <param name="code">HTTP status code for the exception.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        /// <param name="innerException">Inner exception that caused this exception.</param>
        protected ApiException(HttpStatusCode code, string message, string messageKey, Exception? innerException = null)
            : base(message, innerException)
        {
            MessageKey = messageKey;
            Code = code;
        }
    }
}
