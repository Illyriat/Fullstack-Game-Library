using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 403 Forbidden errors.
    /// </summary>
    [Serializable]
    public class ForbiddenException : ApiException
    {
        /// <summary>
        /// Creates a forbidden exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public ForbiddenException(string message, string messageKey = "key_forbiddenException")
            : base(HttpStatusCode.Forbidden, message, messageKey)
        {
        }

        /// <summary>
        /// Creates a forbidden exception with a default message.
        /// </summary>
        public ForbiddenException()
            : base(HttpStatusCode.Forbidden, "Forbidden", "key_forbiddenException")
        {
        }
    }
}
