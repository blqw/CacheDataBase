using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 表示缓存唯一键
    /// <para> 可使用 <see cref="BindableAttribute.Yes"/> 代替 </para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property)]
    public sealed class CacheUniqueKeyAttribute : Attribute
    {

        /// <summary>
        /// 判断当前属性是否属于唯一键
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsUniqueKey(PropertyInfo property)
        {
            
            return property.GetCustomAttribute<BindableAttribute>()?.Bindable
                ?? property.IsDefined(typeof(CacheUniqueKeyAttribute));
        }
    }
}
