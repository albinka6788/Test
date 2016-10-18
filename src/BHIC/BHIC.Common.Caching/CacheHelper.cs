#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

#endregion

namespace BHIC.Common.Caching
{
    public sealed class CacheHelper
    {
        #region InvalidCacheParams

        public const string NullCacheParam = "key and value is required";
        public const string NullKey = "key is null";

        #endregion

        #region Static Variables

        static readonly ObjectCache Cache = MemoryCache.Default;
        private static CacheHelper instance = null;
        private static readonly object initLock = new object();
        private static readonly object addLock = new object();
        private static readonly object addPolicyLock = new object();
        private static readonly object removeLock = new object();
        private static readonly object removeAllLock = new object();
        private const int EXPIRATION_TIME = 30;

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor for creating singleton instance
        /// </summary>
        private CacheHelper()
        {
        }

        #endregion

        #region Instance

        /// <summary>
        /// Returns the instance of CacheHelper class
        /// </summary>
        public static CacheHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (initLock)
                    {
                        if (instance == null)
                        {
                            instance = new CacheHelper();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Public Methods

        #region Get

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <returns>Cached item as type</returns>
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception(NullKey);
            }
            else
            {
                return (T)Cache[key];
            }
        }

        /// <summary>
        /// Returns the list of cache
        /// </summary>
        /// <returns></returns>
        public int GetCacheLength()
        {
            return GetAll().Count;
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool IsExists(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// Gets all cached items as a list by their key.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAll()
        {
            return Cache.Select(keyValuePair => keyValuePair.Key).ToList();
        }

        #endregion

        #region Add

        public void Add<T>(string key, T objectToCache) where T : class
        {
            Add<T>(key, objectToCache, null);
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <param name="key">Name of item</param>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="policy">Cache expiration details</param>
        public void Add<T>(string key, T objectToCache, CacheItemPolicy policy) where T : class
        {
            if(policy == null)
            {
                policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(EXPIRATION_TIME);
            }

            //if parameters are not provided, throw exception
            if (!string.IsNullOrWhiteSpace(key))
            {
                lock (addPolicyLock)
                {
                    Cache.Add(key, objectToCache, policy);
                }
            }
            else
            {
                throw new Exception(NullCacheParam);
            }
        }

        /// <summary>
        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <param name="key">Name of item</param>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="expirationDuration">Cache expiration details</param>
        public void Add<T>(string key, T objectToCache, double expirationDuration) where T : class
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationDuration);
            Add(key, objectToCache, policy);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Remove(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                lock (removeLock)
                {
                    Cache.Remove(key);
                }
            }
            else
            {
                throw new Exception(NullKey);
            }
        }

        /// <summary>
        /// Remove all items from cache list
        /// </summary>
        public void RemoveAll()
        {
            //get all cache item to be removed
            var result = GetAll();

            if (result.Count > 0)
            {
                lock (removeAllLock)
                {
                    result.ForEach(res => Cache.Remove(res));
                }
            }
        }

        #endregion

        #endregion
    }
}
