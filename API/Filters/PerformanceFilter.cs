using API.Controllers;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PerformanceFilter : ActionFilterAttribute
    {
        private ControllerBase _controllerBase { get; set; }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _controllerBase = actionContext.ControllerContext.Controller as ControllerBase;
            _controllerBase.StopWatch.Start();
            base.OnActionExecuting(actionContext);
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _controllerBase.StopWatch.Stop();
            _controllerBase.JsonResult.ProcessingTime = $"{Math.Floor(_controllerBase.StopWatch.Elapsed.TotalMinutes)}m{Math.Floor(_controllerBase.StopWatch.Elapsed.TotalSeconds)}s";
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}