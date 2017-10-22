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
    public class MemoryWatchAttribute : ActionFilterAttribute
    {
        public Int64 Before { get; private set; }
        public Int64 After { get; private set; }
        public Int64 Change { get; private set; }

        public Boolean CalculateFromGC { get; set; } = false;

        public MemoryWatchAttribute()
        {
            // Do something here in the future?
        }

        public MemoryWatchAttribute(bool calculateFromGC)
        {
            this.CalculateFromGC = calculateFromGC;
        }

        #region Private methods
        private Int64 CalculateMemory()
        {
            this.Change = this.After - this.Before;

            this.PrintUsage();

            return this.Change;
        }

        private void PrintUsage()
        {
            var now = DateTime.Now;
            Debug.WriteLine($"{now} [{nameof(this.Before)}] - Memory used: {this.Before}");
            Debug.WriteLine($"{now} [{nameof(this.After)}] - Memory used: {this.After}");
            Debug.WriteLine($"{now} [{nameof(this.Change)}] - Memory used: {this.Change}");
        }

        private void SetBefore()
        {
            if (this.CalculateFromGC)
            {
                this.Before = GC.GetTotalMemory(true);
            }
            else
            {
                this.Before = Process.GetCurrentProcess().PrivateMemorySize64;
            }
        }

        private void SetAfter()
        {
            if (this.CalculateFromGC)
            {
                this.After = GC.GetTotalMemory(true);
            }
            else
            {
                this.After = Process.GetCurrentProcess().PrivateMemorySize64;
            }
        }
        #endregion

        #region Sync methods
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            this.SetBefore();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            this.SetAfter();
            this.CalculateMemory();

            base.OnActionExecuted(actionExecutedContext);
        }
        #endregion

        #region Async methods
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            this.SetBefore();

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            this.SetAfter();
            this.CalculateMemory();

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
        #endregion
    }
}
