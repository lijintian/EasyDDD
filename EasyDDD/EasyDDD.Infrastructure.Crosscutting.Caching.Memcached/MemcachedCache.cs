using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace EasyDDD.Infrastructure.Crosscutting.Caching.Memcached
{
    public class MemcachedCache : ICache
    {
        private readonly MemcachedClient _client;
        public MemcachedCache()
        {
            _client = new MemcachedClient();
        }

        public T Get<T>(string key)
        {
            return _client.Get<T>(key);
        }

        public void Set(string key, object data, int cacheTimeInMinute)
        {
            _client.Store(StoreMode.Set, key, data, DateTime.Now.AddMinutes(cacheTimeInMinute));
        }

        public bool IsSet(string key)
        {
            object data;
            return _client.TryGet(key, out data);
        }

        public void Remove(string key)
        {
            _client.Remove(key);
        }

        public void Clear()
        {
            _client.FlushAll();
        }
    }
}
