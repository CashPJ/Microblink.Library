using Microblink.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microblink.Common.Logging
{
    public class ResponseLog
    {
        private readonly IHeaderDictionary _headers;
        private readonly string _body;

        public readonly int StatusCode;
        public readonly long? ContentLength;
        public readonly string ContentType;

        public IEnumerable<string> Headers => _headers.Select(x => $"{x.Key} : {x.Value}").ToList();
        public string Body => _body;

        public ResponseLog(HttpResponse response)
        {
            var responseBody = response.GetBodyString();

            if (responseBody.Contains("accessToken", StringComparison.InvariantCultureIgnoreCase))
            {
                var pattern = "(\\\"accessToken\\\":\\\")[^\\\"]*(\\\")";
                responseBody = Regex.Replace(responseBody, pattern, "$1***REDACTED***$2", RegexOptions.IgnoreCase);
            }

            _headers = response.Headers;
            _body = responseBody;

            ContentLength = response.ContentLength;
            ContentType = response.ContentType;
            StatusCode = response.StatusCode;
        }
    }
}
