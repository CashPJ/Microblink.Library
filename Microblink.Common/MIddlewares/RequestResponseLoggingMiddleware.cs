using Microblink.Common.Common.Helpers;
using Microblink.Common.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microblink.Common.Common.Middlewares
{
    /// <summary>
    /// RequestResponseLoggingMiddleware is middleware responsible for logging all requests and responses and adding request-id in headers
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        private int _requestId = IdGenerator.GetId();

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //Creating request id for easier tracking            
            SetRequestId(context);
            
            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Adding request id to response
                context.Response.Headers.TryAdd(Constants.HttpHeaderNameRequestId, _requestId.ToString());

                //Save log to chosen datastore
                await Log(context.Request, context.Response);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        //todo: abstrack implementation of log and loglevel
        private async Task Log(HttpRequest request, HttpResponse response)
        {
            //Initialize log
            var log = new RequestResponseLog(request, response);

            //And finally, trigger logging
            await Task.Run(() => _logger.Log(log.LogLevel, "Logging request and response => " + JsonConvert.SerializeObject(log)));
        }

        /// <summary>
        /// Sets generated id on request http header or, if already exists, gets value from header
        /// </summary>
        /// <param name="context"></param>
        protected void SetRequestId(HttpContext context)
        {
            if (context == null)
                return;

            var headerAdded = context.Request.Headers.TryAdd(Constants.HttpHeaderNameRequestId, _requestId.ToString());
            if (!headerAdded)
            {
                var haveValue = context.Request.Headers.TryGetValue("", out StringValues value);
                if (haveValue)
                {
                    int.TryParse(value.ToString(), out _requestId);
                }
            }
            
        }
    }
}
