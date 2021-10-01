using System.Linq;
using System.Reflection;
using Core.Utilities;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Core.CrossCuttingConcerns.Logging.Models;

namespace Core.Aspects.ApiFilters
{
	public class ExceptionLogFilter : ExceptionFilterAttribute
	{
		readonly ILogger<ExceptionLogFilter> _logger;
		public ExceptionLogFilter(ILogger<ExceptionLogFilter> logger)
		{
			_logger = logger;
		}

		public override void OnException(ExceptionContext actionExecutedContext)
		{
			if (FilterCheck((ControllerActionDescriptor)actionExecutedContext.ActionDescriptor))
			{
				var values = new Dictionary<string, object>();
				foreach (var parameter in actionExecutedContext.RouteData.Values.Where(x => x.Key != "action" && x.Key != "controller"))
				{
					values.Add(parameter.Key, parameter.Value);
				}
				var actionDescriptor = actionExecutedContext.ActionDescriptor as ControllerActionDescriptor;
				var exceptionLog = new ExceptionLog(actionDescriptor.ControllerName, actionDescriptor.ActionName, values.ToJson(true), actionExecutedContext.Exception);

				_logger.LogError(exceptionLog.ToJson(true));
			}
			base.OnException(actionExecutedContext);
		}

		private bool FilterCheck(ControllerActionDescriptor actionDescriptor)
		{
			return actionDescriptor.MethodInfo.GetCustomAttributes(typeof(SkipGlobalLogFilter)).Count() == 0;
		}
	}
}
