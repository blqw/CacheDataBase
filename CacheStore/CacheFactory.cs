using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 缓存实例创建类
    /// </summary>
    public static class CacheFactory
    {
        /// <summary>
        /// 创建缓存实例
        /// </summary>
        /// <param name="cacheType">缓存类型,必须实现<see cref="ObjectCache"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="cacheType"/>为空</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cacheType"/>必须实现<see cref="ObjectCache"/></exception>
        /// <returns></returns>
        public static ObjectCache CreateCache(Type cacheType)
        {
            if (cacheType == null)
            {
                throw new ArgumentNullException(nameof(cacheType));
            }
            if (typeof(ObjectCache).IsAssignableFrom(cacheType) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(cacheType), $"必须实现 {nameof(ObjectCache)}");
            }
            return Activator.CreateInstance(cacheType) as ObjectCache;
        }

        /// <summary>
        /// 创建缓存实例
        /// </summary>
        /// <param name="cacheTypeName">缓存类型名称,必须实现<see cref="ObjectCache"/></param>
        /// <param name="throwOnNotExist">是否在没有找到类型的情况下抛出异常</param>
        /// <exception cref="ArgumentNullException"><paramref name="cacheTypeName"/>为空</exception>
        /// <exception cref="TypeLoadException">没有找到类型 且 参数<paramref name="throwOnNotExist"/>等于<seealso cref="true"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cacheType"/>必须实现<see cref="ObjectCache"/></exception>
        /// <returns></returns>
        public static ObjectCache CreateCache(string cacheTypeName, bool throwOnNotExist)
        {
            
            if (cacheTypeName == null)
            {
                throw new ArgumentNullException(nameof(cacheTypeName));
            }
            var cacheType = Type.GetType(cacheTypeName);
            if (cacheType == null)
            {
                if (throwOnNotExist)
                {
                    throw new TypeLoadException($"没有找到({cacheTypeName})类型");
                }
            }
            return CreateCache(cacheType);
        }

        /// <summary>
        /// 创建缓存仓库
        /// </summary>
        /// <typeparam name="T">缓存仓库实体类型</typeparam>
        /// <param name="cacheType">缓存类型,必须实现<see cref="ObjectCache"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="cacheType"/>为空</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cacheType"/>必须实现<see cref="ObjectCache"/></exception>
        /// <returns></returns>
        public static ICacheStore<T> CreateStore<T>(Type cacheType)
            where T : class
        {
            var cache = CreateCache(cacheType);
            return new CacheStore<T>(cache);
        }

        /// <summary>
        /// 创建缓存仓库
        /// </summary>
        /// <typeparam name="T">缓存仓库实体类型</typeparam>
        /// <param name="cacheTypeName">缓存类型名称,必须实现<see cref="ObjectCache"/></param>
        /// <param name="throwOnNotExist">是否在没有找到类型的情况下抛出异常</param>
        /// <exception cref="ArgumentNullException"><paramref name="cacheTypeName"/>为空</exception>
        /// <exception cref="TypeLoadException">没有找到类型 且 参数<paramref name="throwOnNotExist"/>等于<seealso cref="true"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cacheType"/>必须实现<see cref="ObjectCache"/></exception>
        /// <returns></returns>
        public static ICacheStore<T> CreateStore<T>(string cacheTypeName, bool throwOnNotExist)
            where T : class
        {
            var cache = CreateCache(cacheTypeName, throwOnNotExist);
            if (cache == null)
            {
                return null;
            }
            return new CacheStore<T>(cache);
        }

        /// <summary>
        /// 创建缓存仓库
        /// </summary>
        /// <typeparam name="T">缓存仓库实体类型</typeparam>
        /// <param name="cacheProvider">缓存提供程序</param>
        /// <returns></returns>
        public static ICacheStore<T> CreateStore<T>(ObjectCache cacheProvider)
            where T : class
        {
            return new CacheStore<T>(cacheProvider);
        }
        
    }
}
