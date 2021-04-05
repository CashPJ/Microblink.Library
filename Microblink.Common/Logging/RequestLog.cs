using Microblink.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microblink.Common.Logging
{
    public class RequestLog
    {
        //private readonly IHeaderDictionary _headers;
        private readonly IRequestCookieCollection _cookies;
        private readonly IFormCollection _form;
        private readonly string _body;

        public readonly string Url;
        public readonly string IpAddress;
        public readonly long? ContentLength;
        public readonly string ContentType;
        public readonly string Method;
        public readonly IEnumerable<string> Headers;

        public IEnumerable<string> Cookies => _cookies.Select(x => $"{x.Key} : {x.Value}").ToList();
        public IEnumerable<string> Form => _form?.Select(x => $"{x.Key} : {x.Value}").ToList();
        public string Body => _body;

        public RequestLog(HttpRequest request)
        {
            var requestBody = request.GetBodyString();

            //Redact password from json body
            if (requestBody.Contains("password", StringComparison.InvariantCultureIgnoreCase))
            {
                var pattern = "(\\\"password\\\":\\\")[^\\\"]*(\\\")";
                requestBody = Regex.Replace(requestBody, pattern, "$1***REDACTED***$2", RegexOptions.IgnoreCase);
            }

            //Redact Authorization from headers
            Headers = request.Headers.Select(x => x.Key.Equals("Authorization", StringComparison.InvariantCultureIgnoreCase) ? $"{x.Key} : {x.Value.ToString().Obfuscate(obfuscationTerm: "***REDACTED***")}" : $"{x.Key} : {x.Value}").ToList();

            //_headers = request.Headers;
            _cookies = request.Cookies;
            _form = request.HasFormContentType ? request.Form : null;
            _body = requestBody;

            ContentLength = request.ContentLength;
            ContentType = request.ContentType;
            Method = request.Method;

            Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            IpAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
