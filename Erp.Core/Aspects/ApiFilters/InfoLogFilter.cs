using System.Linq;
using System.Reflection;
using Core.Utilities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Core.CrossCuttingConcerns.Logging.Models;

namespace Core.Aspects.ApiFilters
{
	public class InfoLogFilter : ActionFilterAttribute
	{
		readonly ILogger<InfoLogFilter> _logger;
		public InfoLogFilter(ILogger<InfoLogFilter> logger)
		{
			_logger = logger;
		}
		public override async Task OnActionExecutionAsync(ActionExecutingContext actionExecutedContext, ActionExecutionDelegate next)
		{
			var actionDescriptor = actionExecutedContext.ActionDescriptor as ControllerActionDescriptor;
			var result = actionExecutedContext.Result == null ? "Success" : "Failed";
			var infoLog = new InfoLog(actionDescriptor.ControllerName, actionDescriptor.ActionName, actionExecutedContext.ActionArguments.ToJson(true), result);

			if (FilterCheck((ControllerActionDescriptor)actionExecutedContext.ActionDescriptor))
			{
				_logger.LogInformation(infoLog.ToJson(true));
			}

			await next();
		}

		private bool FilterCheck(ControllerActionDescriptor actionDescriptor)
		{
			return actionDescriptor.MethodInfo.GetCustomAttributes(typeof(SkipGlobalLogFilter)).Count() == 0;
		}
	}
}
