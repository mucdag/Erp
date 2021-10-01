using Core.CrossCuttingConcerns.Caching.Interfaces;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;

namespace Core.Aspects.Autofac.CacheAspects
{
    public class CacheRemoveAspect : MethodInterception
    {
        private ICacheManager _cacheManager;

        private readonly string[] _patterns;
        //private readonly bool _includeDefaultPattern;

        public CacheRemoveAspect(string[] patterns)
        {
            _patterns = patterns;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            //if (_patterns != null && _patterns.Length == 0 || _includeDefaultPattern)
            if (_patterns != null && _patterns.Length == 0)
                if (invocation.Method.ReflectedType != null)
                    _cacheManager.RemoveByPattern(
                        $"{invocation.Method.ReflectedType.Namespace}.{invocation.Method.ReflectedType.Name}.*");

            if (_patterns != null)
                foreach (var pattern in _patterns)
                    _cacheManager.RemoveByPattern(pattern);
        }

    }
}
