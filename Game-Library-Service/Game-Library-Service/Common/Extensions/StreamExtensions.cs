using System.Text;

namespace Game_Library_Service.Common.Extensions
{
    /// <summary>
    /// Extension methods for Stream operations.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the stream content as a string asynchronously.
        /// </summary>
        /// <param name="requestBodyStream">The stream to read.</param>
        /// <returns>The stream content as a string.</returns>
        public static async Task<string> ReadRequestBodyAsString(this Stream requestBodyStream)
        {
            requestBodyStream.Position = 0;

            using var requestBodyStreamReader = new StreamReader(requestBodyStream, Encoding.UTF8);
            var requestBody = await requestBodyStreamReader.ReadToEndAsync();

            requestBodyStream.Position = 0;

            return requestBody;
        }
    }
}
