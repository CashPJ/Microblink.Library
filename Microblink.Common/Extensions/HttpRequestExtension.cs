using Microblink.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace Microblink.Extensions
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// Returns Request Body string from Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        public async static Task<string> GetBodyStringAsync(this HttpRequest request)
        {
            var bodyString = string.Empty;

            using (var memoryReader = new MemoryStream())
            using (var reader = new StreamReader(memoryReader))
            {
                request.Body.CopyTo(memoryReader);
                memoryReader.Seek(0, SeekOrigin.Begin);
                bodyString = await reader.ReadToEndAsync();
            }

            return bodyString;
        }

        /// <summary>
        /// Returns Request Body string from Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string</returns>
        public static string GetBodyString(this HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            var bodyString = string.Empty;

            using (var memoryReader = new MemoryStream())
            using (var reader = new StreamReader(memoryReader))
            {
                request.Body.Position = 0;
                request.Body.CopyToAsync(memoryReader);
                memoryReader.Position = 0;
                memoryReader.Seek(0, SeekOrigin.Begin);
                bodyString = reader.ReadToEndAsync().GetAwaiter().GetResult();
                request.Body.Position = 0;
            }

            return bodyString;
        }


        public static int GetRequestId(this HttpRequest request)
        {
            var requestIdHeaderName = Constants.HttpHeaderNameRequestId;
            var requestId = 0;

            if (request?.HttpContext == null)
                return requestId;

            //reading from http request headers
            var haveValue = request.HttpContext.Request.Headers.TryGetValue(requestIdHeaderName, out StringValues value);
            if (haveValue)
            {
                var parsed = int.TryParse(value.ToString(), out requestId);
                if (parsed)
                {
                    return requestId;
                }
            }

            //generating new request id
            requestId = Guid.NewGuid().ToString().GetHashCode();

            //adding newly generated requestid to http header
            var headerAdded = request.HttpContext.Request.Headers.TryAdd(requestIdHeaderName, requestId.ToString());
            if (!headerAdded)
            {
                throw new Exception($"Can not add {requestIdHeaderName} to Http Headers.");
            }

            return requestId;
        }
    }
}
