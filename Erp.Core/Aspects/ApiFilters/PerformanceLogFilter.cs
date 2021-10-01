using Core.CrossCuttingConcerns.Logging.Models;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Aspects.ApiFilters
{
	public class PerformanceLogFilter : ActionFilterAttribute
	{
		private Stopwatch _stopwatch;
		private readonly int _interval = 5;
		readonly ILogger<PerformanceLogFilter> _logger;
		public PerformanceLogFilter(ILogger<PerformanceLogFilter> logger)
		{
			_logger = logger;
		}

		public override async Task OnActionExecutionAsync(ActionExecutingContext actionExecutedContext, ActionExecutionDelegate next)
		{
			_stopwatch = new Stopwatch();
			if (FilterCheck((ControllerActionDescriptor)actionExecutedContext.ActionDescriptor))
			{
				_stopwatch.Start();
			}
			await next();

			if (FilterCheck((ControllerActionDescriptor)actionExecutedContext.ActionDescriptor))
			{
				_stopwatch.Stop();

				if (_stopwatch.Elapsed.TotalSeconds > _interval)
				{
					var totalSeconds = (int)_stopwatch.Elapsed.TotalSeconds;
					var result = $"Process Result: {(actionExecutedContext.Result == null ? "Success" : "Failed")}";

					var actionDescriptor = actionExecutedContext.ActionDescriptor as ControllerActionDescriptor;
					var performanceLog = new IotPerformanceLog(actionDescriptor.ControllerName, actionDescriptor.ActionName, totalSeconds, actionExecutedContext.ActionArguments.ToJson(true), result);

					LogContext.PushProperty("Timing", totalSeconds);
					_logger.LogWarning(performanceLog.ToJson(true));
				}
			}

			_stopwatch.Reset();
		}

		private bool FilterCheck(ControllerActionDescriptor actionDescriptor)
		{
			return actionDescriptor.MethodInfo.GetCustomAttributes(typeof(SkipGlobalLogFilter)).Count() == 0;
		}
	}
}
