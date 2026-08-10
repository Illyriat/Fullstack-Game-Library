using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 409 Conflict errors.
    /// </summary>
    [Serializable]
    public class ConflictException : ApiException
    {
        /// <summary>
        /// Creates a conflict exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public ConflictException(string message, string messageKey = "key_conflictException")
            : base(HttpStatusCode.Conflict, message, messageKey)
        {
        }

        /// <summary>
        /// Creates a conflict exception with a default message.
        /// </summary>
        public ConflictException()
            : base(HttpStatusCode.Conflict, "Already exists", "key_conflictException")
        {
        }
    }
}
