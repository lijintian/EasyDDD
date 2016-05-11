using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace EasyDDD.Infrastructure.Crosscutting.Caching
{
    public static class CacheManager
    {
        private static ICache InternalCache
        {
            [DebuggerStepThrough]
            get { return IoC.Resolve<ICache>(); }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public static T Get<T>(string key)
        {
            return InternalCache.Get<T>(key);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTimeInMinute">Cache time</param>
        public static void Set(string key, object data, int cacheTimeInMinute)
        {
            InternalCache.Set(key, data,cacheTimeInMinute);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public static bool IsSet(string key)
        {
            return InternalCache.IsSet(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public static void Remove(string key)
        {
            InternalCache.Remove(key);
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public static void Clear()
        {
            InternalCache.Clear();
        }
    }
}
