using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace blqw.Caching
{
    /// <summary>
    /// 表示缓存键
    /// <para> 可使用 <see cref="DataObjectFieldAttribute"/>.PrimaryKey = true 代替 </para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property)]
    public sealed class CacheKeyAttribute : Attribute
    {

        /// <summary>
        /// 判断当前属性是否属于缓存键
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsKey(PropertyInfo property)
        {
            return property.GetCustomAttribute<DataObjectFieldAttribute>()?.PrimaryKey
                ?? property.IsDefined(typeof(CacheKeyAttribute));
        }
    }
}
