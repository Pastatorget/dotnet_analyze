using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace dotnet_analyze
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private long _elapsedTime;

        /// <summary>
        /// log4net class for logging messages
        /// </summary>
        public ILog _logger { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var timer = Stopwatch.StartNew();
            actionContext.Request.Properties["logtimer"] = timer;
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var timer = Stopwatch.StartNew();
            actionContext.Request.Properties["logtimer"] = timer;

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                var timer = ((Stopwatch)actionExecutedContext.Request.Properties["logtimer"]);
                timer.Stop();
                _elapsedTime = timer.ElapsedMilliseconds;

                var message = GetMessage(actionExecutedContext);
                _logger.Info(message);
            }
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Response != null)
            {
                var timer = ((Stopwatch)actionExecutedContext.Request.Properties["logtimer"]);
                timer.Stop();
                _elapsedTime = timer.ElapsedMilliseconds;

                var message = GetMessage(actionExecutedContext);
                _logger.Info(message);
            }

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        private string GetMessage(HttpActionExecutedContext context, Boolean toJson = false)
        {
            var logMsg = new Resources.LogMessage()
            {
                StatusCode = (int)context.Response.StatusCode,
                RequestMethod = context.Request.Method.Method,
                RequestUri = context.Request.RequestUri.LocalPath,
                Message = context.Response.StatusCode.ToString(),
                ElapsedTime = _elapsedTime
            };

            if (toJson) return logMsg.ToJson();
            else return logMsg.ToString();
        }
    }
}
