using blqw.CacheStoreComponent;
using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Reflection;

namespace blqw
{
    /// <summary>
    /// 表示属性的Get操作
    /// </summary>
    class PropertyGetter
    {
        public static bool IsInitialized { get; } = Initialize();

        private static bool Initialize()
        {
            MEFPart.Import(typeof(PropertyGetter));
            return true;
        }
        private PropertyGetter()
        {

        }

        [Import("CreateGetter")]
        public static Func<MemberInfo, Func<object, object>> GetGeter;

        public static PropertyGetter Create(PropertyInfo property)
        {
            var getter = new PropertyGetter();
            getter.Name = property.Name;
            if (GetGeter != null)
            {
                getter.GetValue = GetGeter(property);
                return getter;
            }
            var o = Expression.Parameter(typeof(object), "o");
            var cast = Expression.Convert(o, property.DeclaringType);
            var p = Expression.Property(cast, property);
            if (property.CanRead)
            {
                var ret = Expression.Convert(p, typeof(object));
                var get = Expression.Lambda<Func<object, object>>(ret, o);
                getter.GetValue = get.Compile();
            }
            return getter;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// 获取属性的值
        /// </summary>
        public Func<object, object> GetValue { get; private set; }
    }
}
