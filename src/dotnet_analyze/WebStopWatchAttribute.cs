using dotnet_analyze.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace dotnet_analyze
{
    public class WebStopWatchAttribute : ActionFilterAttribute
    {
        private Stopwatch sw = null;
        private DiagnosticsHelper helper = null;

        public WebStopWatchAttribute()
        {
            this.sw = new Stopwatch();
            this.helper = new DiagnosticsHelper();
        }

        #region Sync methods
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            this.sw.Start();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            this.helper.WriteToConsole($"Time taken to execute {actionExecutedContext.ActionContext.ActionDescriptor.ActionName}: {this.sw.Elapsed}");
            this.sw.Stop();
            base.OnActionExecuted(actionExecutedContext);
        }
        #endregion

        #region Async methods
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            this.sw.Start();
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            this.helper.WriteToConsole($"Time taken to execute {actionContext.ActionDescriptor.ActionName}: {this.sw.Elapsed}");
            this.sw.Stop();
            this.sw.Reset();
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
        #endregion
    }
}
