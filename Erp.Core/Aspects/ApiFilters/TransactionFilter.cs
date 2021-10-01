using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace Core.Aspects.ApiFilters
{
    public class TransactionFilter : ActionFilterAttribute
    {
        TransactionScope? scope;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var method = context.HttpContext.Request.Method.ToLower();
            if (method == "post" || method == "put" || method == "delete")
            {
                scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            }
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result;
            // Do something with Result.
            if (scope != null)
                if (context.Canceled == true)
                {
                    scope.Dispose();
                }
                else if (context.Exception != null)
                {
                    scope.Dispose();
                }
                else
                {
                    scope.Complete();
                    scope.Dispose();
                }

            base.OnActionExecuted(context);
        }
    }
}
