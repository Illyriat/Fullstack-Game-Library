using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 401 Unauthorized errors.
    /// </summary>
    [Serializable]
    public class NotAuthorizedException : ApiException
    {
        /// <summary>
        /// Creates an unauthorized exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public NotAuthorizedException(string message, string messageKey = "key_notAuthorizedException")
            : base(HttpStatusCode.Unauthorized, message, messageKey)
        {
        }

        /// <summary>
        /// Creates an unauthorized exception with a default message.
        /// </summary>
        public NotAuthorizedException()
            : base(HttpStatusCode.Unauthorized, "Not authorized", "key_notAuthorizedException")
        {
        }
    }
}
