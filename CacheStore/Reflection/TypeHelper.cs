using blqw.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    /// <summary>
    /// 类型操作帮助静态类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class TypeHelper<T>
    {
        /// <summary>
        /// 主键列名称
        /// </summary>
        public readonly static string[] PrimaryKeyNames = GetPrimaryKeyFields();

        /// <summary>
        /// 唯一键名称
        /// </summary>
        public readonly static string[] UniqueKeyNames = GetUniqueKeyFields();

        /// <summary>
        /// 表名
        /// </summary>
        public readonly static string TableName = GetTableName();

        /// <summary>
        /// 主键Getter访问器
        /// </summary>
        public readonly static PropertyGetter[] PrimaryKeyGetters = GetPrimaryKeyGetters();

        /// <summary>
        /// 唯一键Getter访问器
        /// </summary>
        public readonly static PropertyGetter[] UniqueKeyGetters = GetUniqueKeyGetters();

        private static PropertyGetter[] GetUniqueKeyGetters()
        {
            return typeof(T).GetType().GetProperties()
                                    .Where(CacheUniqueKeyAttribute.IsUniqueKey)
                                    .Select(PropertyGetter.Create)
                                    .ToArray();
        }

        private static PropertyGetter[] GetPrimaryKeyGetters()
        {
            return typeof(T).GetType().GetProperties()
                                    .Where(CacheKeyAttribute.IsKey)
                                    .Select(PropertyGetter.Create)
                                    .ToArray();
        }

        private static string GetTableName()
        {
            return CacheTableAttribute.GetName(typeof(T));
        }

        private static string[] GetPrimaryKeyFields()
        {
            return typeof(T).GetType().GetProperties()
                                    .Where(CacheKeyAttribute.IsKey)
                                    .Select(p => p.Name).ToArray();
        }

        private static string[] GetUniqueKeyFields()
        {
            return typeof(T).GetType().GetProperties()
                                    .Where(CacheUniqueKeyAttribute.IsUniqueKey)
                                    .Select(p => p.Name).ToArray();
        }
    }
}
