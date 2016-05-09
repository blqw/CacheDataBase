using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 缓存数据库接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheStore<T>
        where T : class
    {
        /// <summary>
        /// 缓存提供程序
        /// </summary>
        ObjectCache Provider { get; }

        /// <summary>
        /// 主键或联合主键
        /// </summary>
        string[] PrimaryKeyFields { get; }
        /// <summary>
        /// 辅助键
        /// </summary>
        string[] UniqueKeyFields { get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        TimeSpan ExpirationTime { get; set; }
        
        /// <summary>
        /// 添加缓存,如果已经存在则返回false
        /// </summary>
        /// <param name="entity">缓存实体</param>
        bool Add(T entity);

        /// <summary>
        /// 替换缓存,如果不存在返回false
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Replace(T entity);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="pkValue">缓存主键</param>
        void Remove(object pkValue);
        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="pkValues">缓存联合主键</param>
        void Removes(params object[] pkValues);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="uniqueKey">辅助键字段</param>
        /// <param name="value">辅助键的值</param>
        void Remove(string uniqueKey, object value);
        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="uniqueKey">唯一键字段</param>
        /// <param name="values">唯一键的值</param>
        void Removes(string uniqueKey, params object[] values);
        
        /// <summary>
        /// 移除完整的缓存,包括缓存和该缓存的索引
        /// </summary>
        /// <param name="pkValue">缓存主键</param>
        void RemoveWhole(object pkValue);
        /// <summary>
        /// 批量移除完整缓存,包括缓存和该缓存的索引
        /// </summary>
        /// <param name="pkValues">缓存联合主键</param>
        void RemovesWhole(params object[] pkValues);
        /// <summary>
        /// 移除完整缓存,包括缓存和该缓存的索引
        /// </summary>
        /// <param name="uniqueKey">辅助键字段</param>
        /// <param name="value">辅助键的值</param>
        void RemoveWhole(string uniqueKey, object value);
        /// <summary>
        /// 批量移除完整缓存,包括缓存和该缓存的索引
        /// </summary>
        /// <param name="uniqueKey">唯一键字段</param>
        /// <param name="values">唯一键的值</param>
        void RemovesWhole(string uniqueKey, params object[] values);
        

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="pkValue">缓存主键</param>
        /// <returns></returns>
        T Get(object pkValue);
        /// <summary>
        /// 批量获取缓存
        /// </summary>
        /// <param name="pkValues">缓存联合主键</param>
        /// <returns></returns>
        IEnumerable<T> Gets(params object[] pkValues);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="uniqueKey">辅助键字段</param>
        /// <param name="value">辅助键的值</param>
        T Get(string uniqueKey, object value);
        /// <summary>
        /// 批量获取缓存
        /// </summary>
        /// <param name="uniqueKey">唯一键字段</param>
        /// <param name="values">唯一键的值</param>
        IEnumerable<T> Gets(string uniqueKey, params object[] values);



        /// <summary>
        /// 获取或添加缓存项
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="getValueHandler">用于获取需要缓存的项</param>
        /// <returns></returns>
        T GetOrAdd(object pkValue, Func<object, T> getValueHandler);
        /// <summary>
        /// 批量获取或添加缓存项
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="getValueHandler">用于获取需要缓存的项</param>
        /// <returns></returns>
        IEnumerable<T> GetsOrAdd(IEnumerable<object> pkValues, Func<object, T> getValueHandler);
        /// <summary>
        /// 批量获取或添加缓存项
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="getValuesHandler">用于批量获取需要缓存的项</param>
        /// <returns></returns>
        IEnumerable<T> GetsOrAdd(IEnumerable<object> pkValues, Func<IEnumerable<object>, IEnumerable<T>> getValuesHandler);
        /// <summary>
        /// 获取或添加缓存项
        /// </summary>
        /// <param name="uniqueKey">唯一键字段</param>
        /// <param name="value">唯一键的值</param>
        /// <param name="getValueHandler">用于获取需要缓存的项</param>
        /// <returns></returns>
        T GetOrAdd(string uniqueKey, object value, Func<object, T> getValueHandler);
        /// <summary>
        /// 批量获取或添加缓存项
        /// </summary>
        /// <param name="uniqueKey">唯一键字段</param>
        /// <param name="value">唯一键的值</param>
        /// <param name="getValueHandler">用于获取需要缓存的项</param>
        /// <returns></returns>
        IEnumerable<T> GetsOrAdd(string uniqueKey, IEnumerable<object> values, Func<object, T> getValueHandler);
        /// <summary>
        /// 批量获取或添加缓存项
        /// </summary>
        /// <param name="uniqueKey">唯一键字段</param>
        /// <param name="value">唯一键的值</param>
        /// <param name="getValueHandler">用于批量获取需要缓存的项</param>
        /// <returns></returns>
        IEnumerable<T> GetsOrAdd(string uniqueKey, IEnumerable<object> values, Func<IEnumerable<object>, IEnumerable<T>> getValuesHandler);
    }
}
