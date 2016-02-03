using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 表示缓存键
    /// <para> 可使用 DataObjectFieldAttribute.PrimaryKey = true 代替 </para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property)]
    public sealed class CacheKeyAttribute : Attribute
    {
        
    }
}
