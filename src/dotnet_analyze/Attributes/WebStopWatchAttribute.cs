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

        public Int32 MinimumTimeTaken { get; set; } = 0;

        public WebStopWatchAttribute()
        {
            this.sw = new Stopwatch();
            this.helper = new DiagnosticsHelper();
        }

        public WebStopWatchAttribute(Int32 timetaken)
        {
            this.MinimumTimeTaken = timetaken;
        }

        private void ResetAndPrint(string methodName)
        {
            // Will only trigger if elapsed time taken exceeds minimum time taken property
            if (this.MinimumTimeTaken > 0 && this.MinimumTimeTaken < this.sw.Elapsed.Seconds)
            {
                this.helper.WriteMessage($"Time taken to execute {methodName}: {this.sw.Elapsed}");
            }

            this.sw.Stop();
            this.sw.Reset();
        }

        #region Sync methods
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            this.sw.Start();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            this.ResetAndPrint(actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
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
            this.ResetAndPrint(actionContext.ActionDescriptor.ActionName);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
        #endregion
    }
}
