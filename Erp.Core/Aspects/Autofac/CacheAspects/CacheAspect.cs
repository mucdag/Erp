using Core.CrossCuttingConcerns.Caching.Interfaces;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;

namespace Core.Aspects.Autofac.CacheAspects
{
    public class CacheAspect : MethodInterception
    {
        private static readonly List<Type> DisallowedTypes = new List<Type>
        {
            typeof(Stream),
            typeof(IQueryable)
        };

        private readonly int _cacheBySeconds;
        private ICacheManager _cacheManager;

        public CacheAspect(int cacheBySeconds = 3600)
        {
            AspectPriority = 10;
            _cacheBySeconds = cacheBySeconds;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            try
            {
                var methodName =
                    $"{invocation.Method.ReflectedType.Namespace}.{invocation.Method.ReflectedType.Name}.{invocation.Method.Name}";
                var arguments = invocation.Arguments.ToList();

                var builder = new StringBuilder();

                arguments.ToList().ForEach(x =>
                {
                    builder.Append("_");
                    builder.Append(JsonConvert.SerializeObject(x));
                });

                var key = $"{methodName}({string.Join(",", builder.ToString() ?? "<Null>")})";

                if (_cacheManager.IsExists(key))
                {
                    //to do
                    //_cacheManager.Get(key, out invocation.ReturnValue);
                }
                else
                {
                    invocation.Proceed();
                    _cacheManager.Add(key, invocation.ReturnValue, _cacheBySeconds);
                }
            }
            catch (Exception)
            {
                invocation.Proceed();
            }
        }
    }
}
