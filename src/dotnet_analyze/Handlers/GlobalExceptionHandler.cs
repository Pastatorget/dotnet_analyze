using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace dotnet_analyze
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private readonly ILog _logger;

        public GlobalExceptionHandler(ILog logger)
        {
            _logger = logger;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            var timer = ((Stopwatch)context.Request.Properties["logtimer"]);
            timer.Stop();

            var exception = context.Exception;
            var httpException = exception as HttpException;
            var logMessage = new Resources.LogMessage
            {
                RequestUri = context.Request.RequestUri.LocalPath,
                RequestMethod = context.Request.Method.Method,
                ElapsedTime = timer.ElapsedMilliseconds
            };

            if (httpException != null)
            {
                context.Result = new Resources.WebErrorMessage(context.Request, (HttpStatusCode)httpException.GetHttpCode(), httpException.Message);
                return;
            }

            logMessage.Message = exception.StackTrace;
            logMessage.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.Error(logMessage.ToString());

            context.Result = new Resources.WebErrorMessage(context.Request, HttpStatusCode.InternalServerError, exception.Message);
        }
    }
}
