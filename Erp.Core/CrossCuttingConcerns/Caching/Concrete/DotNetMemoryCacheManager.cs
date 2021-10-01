using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.CrossCuttingConcerns.Caching.Interfaces;
using Core.Utilities;
using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CrossCuttingConcerns.Caching.Concrete
{
    public class DotNetMemoryCacheManager : ICacheManager
    {
        protected IMemoryCache _cache;
        //private static List<string> keys = new List<string>();
        public DotNetMemoryCacheManager() => _cache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();

        public T Get<T>(string key, out T value)
        {
            value = _cache.Get<T>(key);
            return _cache.Get<T>(key);
        }

        public T Get<T>(string key, Func<T> func, int cacheTimecacheBySeconds = 3600)
        {
            var value = _cache.Get<T>(key);
            if (value != null) return value;
            var result = func();
            Add(key, result);
            return result;
        }

        public void Add(string key, object data, int cacheTimecacheBySeconds = 3600)
        {
            if (data == null)
                throw new Exception("Data boş olamaz", new ArgumentNullException(nameof(data)));

            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Key boş olamaz", new ArgumentNullException(nameof(key)));

            if (cacheTimecacheBySeconds < 0)
                throw new Exception("Cache süresi negatif olamaz",
                    new ArgumentOutOfRangeException(nameof(cacheTimecacheBySeconds)));

            var option = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTimecacheBySeconds)
            };
            //var result = _cache.CreateEntry(key);
            _cache.Set(key, data, option);
            //keys.Add(key);
        }

        public bool IsExists(string key)
        {
            return _cache.TryGetValue(key, out string cacheEntry);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            //keys.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) return;

            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_cache) as dynamic;


            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {

                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);


                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            //var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //var keysToRemove = keys.Where(d => regex.IsMatch(d)).ToList();

            foreach (var key in keysToRemove)
                _cache.Remove(key);
            //Remove(key);
        }

        public void Clear()
        {
            //foreach (var key in keys)
            //    Remove(key);
        }
    }
}