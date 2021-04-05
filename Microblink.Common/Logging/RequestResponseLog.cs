using Microsoft.AspNetCore.Http;

namespace Microblink.Common.Logging
{
    public class RequestResponseLog
    {
        public readonly int RequestId;
        public RequestLog Request;
        public ResponseLog Response;
        public Microsoft.Extensions.Logging.LogLevel LogLevel;

        /// <summary>
        /// Prepared class for logging with formatted Request and Response and generated RequestId which is used for easier tracking
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public RequestResponseLog(HttpRequest request, HttpResponse response)
        {
            RequestId = int.Parse(request.Headers["Request-Id"].ToString() ?? "-1");
            Request = new RequestLog(request);
            Response = new ResponseLog(response);
            LogLevel = DetermineLogLevel(response.StatusCode);
        }

        private Microsoft.Extensions.Logging.LogLevel DetermineLogLevel(int responseCode)
        {
            Microsoft.Extensions.Logging.LogLevel level = Microsoft.Extensions.Logging.LogLevel.Information;

            if (responseCode.ToString().StartsWith("4"))
            {
                level = Microsoft.Extensions.Logging.LogLevel.Warning;
            }
            else if (responseCode.ToString().StartsWith("5"))
            {
                level = Microsoft.Extensions.Logging.LogLevel.Error;
            }

            return level;
        }
    }
}
