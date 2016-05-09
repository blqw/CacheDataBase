using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 缓存实体仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CacheStore<T> : ICacheStore<T>
        where T : class
    {
        public CacheStore(ObjectCache cacheProvider)
        {
            if (cacheProvider == null)
            {
                throw new ArgumentNullException(nameof(cacheProvider));
            }
            Provider = cacheProvider;
            ExpirationTime = new TimeSpan(1, 30, 0);

            var props = typeof(T).GetType().GetProperties();
            PrimaryKeyFields = props.Where(CacheKeyAttribute.IsKey)
                                    .Select(p => p.Name).ToArray();
            UniqueKeyFields = props.Where(CacheUniqueKeyAttribute.IsUniqueKey)
                                    .Select(p => p.Name).ToArray();
        }

        public ObjectCache Provider { get; private set; }
        /// <summary>
        /// 过期时间,默认90分钟
        /// </summary>
        public TimeSpan ExpirationTime { get; set; }
        public string[] PrimaryKeyFields { get; private set; }
        public string[] UniqueKeyFields { get; private set; }

        public partial bool Add(T entity);
        public abstract T Get(object pkValue);
        public abstract T Get(string uniqueKey, object value);
        public abstract T GetOrAdd(object pkValue, Func<object, T> getValueHandler);
        public abstract T GetOrAdd(string uniqueKey, object value, Func<object, T> getValueHandler);
        public abstract IEnumerable<T> Gets(params object[] pkValues);
        public abstract IEnumerable<T> Gets(string uniqueKey, params object[] values);
        public abstract IEnumerable<T> GetsOrAdd(IEnumerable<object> pkValues, Func<IEnumerable<object>, IEnumerable<T>> getValuesHandler);
        public abstract IEnumerable<T> GetsOrAdd(IEnumerable<object> pkValues, Func<object, T> getValueHandler);
        public abstract IEnumerable<T> GetsOrAdd(string uniqueKey, IEnumerable<object> values, Func<IEnumerable<object>, IEnumerable<T>> getValuesHandler);
        public abstract IEnumerable<T> GetsOrAdd(string uniqueKey, IEnumerable<object> values, Func<object, T> getValueHandler);
        public abstract void Remove(object pkValue);
        public abstract void Remove(string uniqueKey, object value);
        public abstract void Removes(params object[] pkValues);
        public abstract void Removes(string uniqueKey, params object[] values);
        public abstract void RemovesWhole(params object[] pkValues);
        public abstract void RemovesWhole(string uniqueKey, params object[] values);
        public abstract void RemoveWhole(object pkValue);
        public abstract void RemoveWhole(string uniqueKey, object value);
        public abstract bool Replace(T entity);
    }
}
