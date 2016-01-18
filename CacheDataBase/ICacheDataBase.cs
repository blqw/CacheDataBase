using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    /// <summary>
    /// 缓存数据库接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ICacheDataBase<T>
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        ObjectCache Cache { get; }
        /// <summary>
        /// 主键或联合主键
        /// </summary>
        string[] PrimaryKeyFields { get; }
        /// <summary>
        /// 唯一键
        /// </summary>
        string[] UniqueKeyFields { get; }

        /// <summary>
        /// 有效时间
        /// </summary>
        TimeSpan ExpirationTime { get; }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="entity">缓存实体</param>
        void Add(T entity);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存主键</param>
        void Remove(object key);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="keys">缓存联合主键</param>
        void Remove(params object[] keys);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="uniqueKeyField">缓存唯一字段</param>
        /// <param name="uniqueValue">唯一字段的值</param>
        void Remove(Expression<Func<T, object>> uniqueKeyField, object uniqueValue);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存主键</param>
        /// <returns></returns>
        T Get(object key);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="keys">缓存联合主键</param>
        /// <returns></returns>
        T Get(params object[] keys);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="uniqueKeyField">缓存唯一字段</param>
        /// <param name="uniqueValue">唯一字段的值</param>
        /// <returns></returns>
        T Get(Expression<Func<T, object>> uniqueKeyField, object uniqueValue);
    }
}
