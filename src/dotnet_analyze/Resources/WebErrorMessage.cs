using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace dotnet_analyze.Resources
{
    public class WebErrorMessage : IHttpActionResult
    {
        private readonly string _errorMesage;
        private readonly HttpRequestMessage _requestMessage;
        private readonly HttpStatusCode _statusCode;

        public WebErrorMessage(HttpRequestMessage requestMessage, HttpStatusCode statusCode, String errorMessage)
        {
            _requestMessage = requestMessage;
            _statusCode = statusCode;
            _errorMesage = errorMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_requestMessage.CreateErrorResponse(_statusCode, _errorMesage));
        }
    }
}
