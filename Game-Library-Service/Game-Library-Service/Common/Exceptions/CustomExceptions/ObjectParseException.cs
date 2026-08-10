using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 400 Bad Request errors during object parsing.
    /// </summary>
    [Serializable]
    public class ObjectParseException : ApiException
    {
        /// <summary>
        /// Creates an object parse exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public ObjectParseException(string message, string messageKey = "key_objectParseException")
           : base(HttpStatusCode.BadRequest, message, messageKey)
        {
        }

        /// <summary>
        /// Creates an object parse exception with a default message.
        /// </summary>
        public ObjectParseException()
            : base(HttpStatusCode.BadRequest, "Failed to parse Object", "key_objectParseException")
        {
        }
    }
}
