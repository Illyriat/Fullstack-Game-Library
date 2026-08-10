using System.Net;

namespace Game_Library_Service.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Exception for HTTP 503 Service Unavailable errors.
    /// </summary>
    [Serializable]
    public class ServiceUnavailableException : ApiException
    {
        /// <summary>
        /// Creates a service unavailable exception with a custom message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="messageKey">Localization key for the message.</param>
        public ServiceUnavailableException(string message, string messageKey = "key_serviceUnavailableException")
            : base(HttpStatusCode.ServiceUnavailable, message, messageKey)
        {
        }

        /// <summary>
        /// Creates a service unavailable exception with a default message.
        /// </summary>
        public ServiceUnavailableException()
            : base(HttpStatusCode.ServiceUnavailable, "Service Unavailable", "key_serviceUnavailableException")
        {
        }
    }
}
