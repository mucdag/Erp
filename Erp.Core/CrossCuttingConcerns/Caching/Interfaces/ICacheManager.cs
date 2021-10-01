using System;

namespace Core.CrossCuttingConcerns.Caching.Interfaces
{
    public interface ICacheManager
    {
        T Get<T>(string key, out T value);
        T Get<T>(string key, Func<T> func, int cacheTimecacheBySeconds = 3600);
        void Add(string key, object data, int cacheTimecacheBySeconds);
        bool IsExists(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        void Clear();
    }
}