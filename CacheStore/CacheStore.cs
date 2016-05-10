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
            PrimaryKeys = TypeHelper<T>.PrimaryKeyNames;
            if (PrimaryKeys.Length <= 0)
            {
                throw new NotImplementedException("必须至少有一个缓存键");
            }
            PrimaryKeys = TypeHelper<T>.PrimaryKeyNames;
            UniqueKeys = TypeHelper<T>.UniqueKeyNames;
            TableName = TypeHelper<T>.TableName;
            _pGetters = TypeHelper<T>.PrimaryKeyGetters;
            _uGetters = TypeHelper<T>.UniqueKeyGetters;
        }

        public ObjectCache Provider { get; private set; }
        /// <summary>
        /// 过期时间,默认90分钟
        /// </summary>
        public TimeSpan ExpirationTime { get; set; }
        public string[] PrimaryKeys { get; private set; }
        public string[] UniqueKeys { get; private set; }
        public string TableName { get; private set; }
        /// <summary>
        /// 主键Getter访问器
        /// </summary>
        PropertyGetter[] _pGetters;

        /// <summary>
        /// 唯一键Getter访问器
        /// </summary>
        PropertyGetter[] _uGetters;

        /// <summary>
        /// 获取指定值的字符串形式
        /// </summary>
        /// <param name="value">需要转为字符串的值</param>
        /// <returns></returns>
        private static string ConvertString(object value)
        {
            if (value == null) return "";
            return (value as IConvertible)?.ToString(null) ??
                   (value as IFormattable)?.ToString(null, null);

        }

        private CacheItem GetCacheItem(string key, object value)
        {
            return new CacheItem($"table {TableName}:\n{key}", value);
        }

        private CacheItemPolicy CachePolicy
        {
            get
            {
                return new CacheItemPolicy()
                {
                    SlidingExpiration = ExpirationTime,
                };
            }
        }

        public partial bool Add(T entity)
        {
            if (entity == null)
            {
                return false;
            }

            var cachekey = "0:" + string.Join("\n", TypeHelper<T>.PrimaryKeyGetters.Select(it => ConvertString(it.GetValue(entity))));
            var item = GetCacheItem(cachekey, entity);
            Provider.AddOrGetExisting(item, CachePolicy);
            for (int i = 0, length = TypeHelper<T>.UniqueKeyGetters.Length; i < length; i++)
            {
                var bk = UniqueKeyFields[i];
                var cachekey2 = $"{bk}={_Gets[bk](entity)}";
                Cache.Set(cachekey2, CachePolicy);
            }
        }
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
