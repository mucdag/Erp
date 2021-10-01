using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Interfaces;
using Core.CrossCuttingConcerns.Logging.Models;
using Core.Utilities;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Core.Aspects.Autofac.LogAspects
{
    public class InfoLogAspect : MethodInterception
    {
        private ILogger _loggerService;

        public InfoLogAspect()
        {
            _loggerService = ServiceTool.ServiceProvider.GetService<ILogger>();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            try
            {
                var logParameters = invocation.Method.GetParameters().Select((t, i) => new LogParameter
                {
                    Name = t.Name,
                    Type = t.ParameterType.Name,
                    Value = invocation.Arguments.GetValue(i)
                }).ToList();

                var logDetail = new InfoLog(
                    invocation.Method.DeclaringType?.FullName,
                    invocation.Method.Name,
                    logParameters.ToJson());
                _loggerService.Add(logDetail);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
