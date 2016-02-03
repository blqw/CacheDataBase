using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 表示缓存绑定键
    /// <para> 可使用 SettingsBindableAttribute 代替 </para>
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property)]
    public sealed class BindKeyAttribute : Attribute
    {

    }
}
