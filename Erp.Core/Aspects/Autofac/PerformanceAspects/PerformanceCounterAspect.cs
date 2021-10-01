using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Interfaces;
using Core.CrossCuttingConcerns.Logging.Models;
using Core.Utilities;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;

namespace Core.Aspects.Autofac.PerformanceAspects
{
    public class PerformanceCounterAspect : MethodInterception
    {
        private readonly int _interval;
        private ILogger _loggerService;
        private Stopwatch _stopwatch;

        public PerformanceCounterAspect(int interval = 5)
        {
            _interval = interval;
            _loggerService = ServiceTool.ServiceProvider.GetService<ILogger>();
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            try
            {
                _stopwatch.Stop();

                if (_stopwatch.Elapsed.TotalSeconds > _interval)
                {
                    var logParameters = invocation.Method.GetParameters().Select((t, i) => new LogParameter
                    {
                        Name = t.Name,
                        Type = t.ParameterType.Name,
                        Value = invocation.Arguments.GetValue(i)
                    }).ToList();

                    var logDetail = new PerformanceLog(invocation.Method.DeclaringType?.FullName,
                        invocation.Method.Name, (int)_stopwatch.Elapsed.TotalSeconds, logParameters.ToJson());
                    _loggerService.Add(logDetail);
                }

                _stopwatch.Reset();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
