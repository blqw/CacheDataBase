using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Caching
{
    /// <summary>
    /// 用于复杂查询的参数
    /// </summary>
    public class SearchArgs : NameObjectCollectionBase
    {
        public object this[string name]
        {
            get { return BaseGet(name); }
            set
            {
                if (value != null
                    && value is IConvertible == false
                    && value is IFormattable == false)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"必须实现接口{nameof(IConvertible)}或{nameof(IFormattable)}");
                }
                BaseSet(name, value);
            }
        }
        
        /// <summary>
        /// 获取根据内容生成的缓存键
        /// </summary>
        public string SearchKey
        {
            get
            {
                return string.Join("\n", BaseGetAllKeys().Select(key => $"{key}:{GetStringValue(key)}"));
            }
        }

        /// <summary>
        /// 获取指定键的值的字符串形式
        /// </summary>
        /// <param name="name">需要返回值的指定键</param>
        /// <returns></returns>
        protected string GetStringValue(string name)
        {
            var value = BaseGet(name);
            if (value == null) return null;
            return (value as IConvertible)?.ToString(null) ??
                   (value as IFormattable)?.ToString(null, null);

        }

    }
}
