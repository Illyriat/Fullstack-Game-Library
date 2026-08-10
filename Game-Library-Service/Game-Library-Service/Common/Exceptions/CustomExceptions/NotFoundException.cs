using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 404 Not Found errors.
    /// </summary>
    [Serializable]
    public class NotFoundException : ApiException
    {
        /// <summary>
        /// Creates a not found exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public NotFoundException(string message, string messageKey = "key_notFoundException")
            : base(HttpStatusCode.NotFound, message, messageKey)
        {
        }

        /// <summary>
        /// Creates a not found exception with a default message.
        /// </summary>
        public NotFoundException()
            : base(HttpStatusCode.NotFound, "Not found", "key_notFoundException")
        {
        }
    }
}
