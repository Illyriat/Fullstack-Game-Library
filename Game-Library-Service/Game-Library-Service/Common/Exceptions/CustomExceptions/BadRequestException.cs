using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 400 Bad Request errors.
    /// </summary>
    [Serializable]
    public class BadRequestException : ApiException
    {
        /// <summary>
        /// Creates a bad request exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public BadRequestException(string message, string messageKey = "key_badRequestException")
            : base(HttpStatusCode.BadRequest, message, messageKey)
        {
        }

        /// <summary>
        /// Creates a bad request exception with a custom message and inner exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception that caused this exception.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public BadRequestException(string message, Exception innerException, string messageKey = "key_badRequestException")
            : base(HttpStatusCode.BadRequest, message, messageKey, innerException)
        {
        }

        /// <summary>
        /// Creates a bad request exception with a default message.
        /// </summary>
        public BadRequestException()
            : base(HttpStatusCode.BadRequest, "Bad request", "key_badRequestException")
        {
        }
    }
}
