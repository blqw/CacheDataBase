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
    /// 表示缓存表
    /// <para> 可使用 <see cref="CategoryAttribute"/> 代替 </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CacheTableAttribute : Attribute
    {
        public CacheTableAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 表示缓存表名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取指定类型的映射表名
        /// </summary>
        /// <param name="classType">需要获取表名的类型</param>
        /// <returns></returns>
        public static string GetName(Type classType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }
            return classType.GetCustomAttribute<CacheTableAttribute>()?.Name
                ?? classType.GetCustomAttribute<CategoryAttribute>()?.Category
                ?? classType.Name;
        }
    }
}
