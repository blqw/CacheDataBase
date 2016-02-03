using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 表示缓存表
    /// <para> 可使用 CategoryAttribute 代替 </para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class)]
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
    }
}
