using Microsoft.AspNetCore.Http;
using System.IO;

namespace Microblink.Extensions
{
    public static class HttpResponseExtension
    {   
        /// <summary>
        /// Returns Response Body string from Body stream
        /// </summary>
        /// <param name="response"></param>
        /// <returns>string</returns>
        public static string GetBodyString(this HttpResponse response)
        {
            var bodyString = string.Empty;
            response.Body.Position = 0;
            using (var memoryReader = new MemoryStream())
            using (var reader = new StreamReader(memoryReader))
            {
                response.Body.CopyToAsync(memoryReader);
                memoryReader.Position = 0;
                memoryReader.Seek(0, SeekOrigin.Begin);
                bodyString = reader.ReadToEndAsync().GetAwaiter().GetResult();
                response.Body.Position = 0;
            }

            return bodyString;
        }
    }
}
