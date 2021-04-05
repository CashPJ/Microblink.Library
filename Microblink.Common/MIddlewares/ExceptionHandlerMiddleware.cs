using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microblink.Common.Common.Helpers;
using Microblink.Extensions;
using Newtonsoft.Json;

namespace Microblink.Common.Middlewares
{

    /// <summary>
    /// Error handler middleware
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private int RequestId;


        /// <summary>
        /// Creates Exception Handler instance
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke context and writes masked exeption code and message
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                RequestId = IdGenerator.GetId();
                //Adding request id to request headers
                context.Request.Headers.TryAdd(Constants.HttpHeaderNameRequestId, RequestId.ToString());

                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} => {e.StackTrace}");

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = context.Response.StatusCode;
                response.Headers.Add(Constants.HttpHeaderNameRequestId, context.Request.GetRequestId().ToString());
                var result = JsonConvert.SerializeObject(new { Error = context.Response.StatusCode, RequestId = context.Request.GetRequestId().ToString() });
                await response.WriteAsync(result);
            }
        }
    }
}
