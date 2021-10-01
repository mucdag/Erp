using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Interfaces;
using Core.CrossCuttingConcerns.Logging.Models;
using Core.Utilities;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Core.Aspects.Autofac.ExceptionAspects
{
    public class ExceptionLogAspect : MethodInterception
    {
        private ILogger _loggerService;

        public ExceptionLogAspect()
        {
            _loggerService = ServiceTool.ServiceProvider.GetService<ILogger>();
        }

        protected override void OnException(IInvocation invocation, Exception e)
        {
            try
            {
                if (_loggerService != null)
                {
                    var logParameters = invocation.Method.GetParameters().Select((t, i) => new LogParameter
                    {
                        Name = t.Name,
                        Type = t.ParameterType.Name,
                        Value = invocation.Arguments.GetValue(i)
                    }).ToList();

                    _loggerService.Add(new ExceptionLog(invocation.Method.DeclaringType?.FullName,
                        invocation.Method.Name, logParameters.ToJson(), e));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
