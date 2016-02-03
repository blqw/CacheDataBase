using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace blqw.Caching
{
    public class CacheStore<T> : ICacheStore<T>
        where T : class
    {
        #region 静态内容
        static CacheStore<T> _Instance;
        public static CacheStore<T> Instance
        {
            get
            {
                if (_Instance != null)
                {
                    return _Instance;
                }
                System.Threading.Interlocked.CompareExchange(ref _Instance, new CacheStore<T>(), null);
                return _Instance;
                //= new CacheStore<T>()
            }
        }
        private readonly static string[] _PrimaryKeyFields = GetPrimaryKeyFields();
        private readonly static string[] _BindKeyFields = GetBindKeyFields();
        private readonly static Dictionary<string, Func<T, object>> _Gets = GetGetHandlers();
        private readonly static string _TableName = GetTableName();


        private static string GetTableName()
        {
            var attr2 = Attribute.GetCustomAttribute(typeof(T), typeof(CacheTableAttribute)) as CacheTableAttribute;
            if (attr2 != null)
            {
                return attr2.Name;
            }
            var attr = Attribute.GetCustomAttribute(typeof(T), typeof(CategoryAttribute)) as CategoryAttribute;
            if (attr != null)
            {
                return attr.Category;
            }
            return typeof(T).Name;
        }

        private static Dictionary<string, Func<T, object>> GetGetHandlers()
        {
            var dict = new Dictionary<string, Func<T, object>>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in typeof(T).GetProperties())
            {
                if (p.GetIndexParameters()?.Length > 0)
                {
                    continue;
                }
                var type = typeof(GetHandler<>).MakeGenericType(typeof(T),p.PropertyType);
                var obj = Activator.CreateInstance(type, p.GetGetMethod());
                var get = (Func<T, object>)obj.GetType().GetMethod("Get").CreateDelegate(typeof(Func<T, object>), obj);
                dict.Add(p.Name, get);
            }
            return dict;
        }

        class GetHandler<R>
        {
            public GetHandler(MethodInfo method)
            {
                _get = (Func<T, R>)method.CreateDelegate(typeof(Func<T, R>));
            }
            Func<T, R> _get;
            public object Get(T t)
            {
                return _get(t);
            }
        }

        private static string[] GetPrimaryKeyFields()
        {
            List<string> pk = new List<string>();
            foreach (var p in typeof(T).GetProperties())
            {
                if (p.GetIndexParameters()?.Length > 0)
                {
                    continue;
                }
                var attr2 = Attribute.GetCustomAttribute(p, typeof(CacheKeyAttribute));
                if (attr2 != null)
                {
                    pk.Add(p.Name);
                    continue;
                }
                var attr = Attribute.GetCustomAttribute(p, typeof(DataObjectFieldAttribute)) as DataObjectFieldAttribute;
                if (attr != null && attr.PrimaryKey)
                {
                    pk.Add(p.Name);
                }
            }

            return pk.ToArray();
        }

        private static string[] GetBindKeyFields()
        {
            List<string> bk = new List<string>();
            foreach (var p in typeof(T).GetProperties())
            {
                if (p.GetIndexParameters()?.Length > 0)
                {
                    continue;
                }
                var attr = Attribute.GetCustomAttribute(p, typeof(BindKeyAttribute)) ?? Attribute.GetCustomAttribute(p, typeof(SettingsBindableAttribute));
                if (attr != null)
                {
                    bk.Add(p.Name);
                }
            }

            return bk.ToArray();
        }

        #endregion

        protected CacheStore()
        {
            Cache = new MemoryCache(Guid.NewGuid().ToString());
            ExpirationTime = new TimeSpan(1, 0, 0);
            if ((PrimaryKeyFields?.Length ?? 0) <= 0)
            {
                throw new NotImplementedException("必须至少有一个缓存键");
            }
            _KeyName = string.Join("|", PrimaryKeyFields);
        }

        #region 属性
        public ObjectCache Cache { get; protected set; }

        public TimeSpan ExpirationTime { get; set; }

        public virtual string[] PrimaryKeyFields { get { return _PrimaryKeyFields; } }

        public virtual string[] BindKeyFields { get { return _BindKeyFields; } }
        #endregion

        private string _KeyName;

        private string GetPrimaryValue(T entity)
        {
            if (entity == null)
            {
                return null;
            }
            if (PrimaryKeyFields.Length == 1)
            {
                return _Gets[PrimaryKeyFields[0]](entity) + "";
            }
            return string.Join("|", PrimaryKeyFields.Select(it => _Gets[it](entity)));
        }

        public virtual void Add(T entity)
        {
            if (entity == null)
            {
                return;
            }
            var pv = GetPrimaryValue(entity);
            var cachekey = $"{_KeyName}={pv}";
            var outtime = DateTimeOffset.Now.Add(ExpirationTime);
            Cache.Set(cachekey, entity, outtime);
            for (int i = 0, length = BindKeyFields.Length; i < length; i++)
            {
                var bk = BindKeyFields[i];
                var cachekey2 = $"{bk}={_Gets[bk](entity)}";
                Cache.Set(cachekey2, pv, outtime);
            }
        }

        public virtual T Get(params object[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return null;
            }
            if (keys.Length != PrimaryKeyFields.Length)
            {
                return null;
            }
            var pv = string.Join("|", keys);
            var cachekey = $"{_KeyName}={pv}";
            return Cache[cachekey] as T;
        }

        public virtual T Get(object key)
        {
            if (key == null || PrimaryKeyFields.Length != 1)
            {
                return null;
            }
            var cachekey = $"{_KeyName}={key}";
            return Cache[cachekey] as T;
        }

        public virtual T Get<R>(Expression<Func<T, R>> bindKeyField, R value)
        {
            var field = Parse<R>(bindKeyField);
            var cacheKey2 = $"{field}={value}";
            var key = Cache[cacheKey2] as string;
            if (key != null)
            {
                return Cache[$"{_KeyName}={key}"] as T;
            }
            return null;
        }

        private string Parse<R>(Expression<Func<T, R>> bindKeyField)
        {
            if (bindKeyField == null || bindKeyField.Body == null)
            {
                return null;
            }
            var member = bindKeyField.Body as MemberExpression;
            if (member != null)
            {
                return member.Member.Name;
            }
            return null;
        }

        public virtual void Remove(params object[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return;
            }
            if (keys.Length != PrimaryKeyFields.Length)
            {
                return;
            }
            var pv = string.Join("|", keys);
            var cachekey = $"{_KeyName}={pv}";
            if (BindKeyFields.Length == 0)
            {
                Cache.Remove(cachekey);
                return;
            }
            var entity = Cache[cachekey] as T;
            if (entity != null)
            {
                for (int i = 0, length = BindKeyFields.Length; i < length; i++)
                {
                    var bk = BindKeyFields[i];
                    var cachekey2 = $"{bk}={_Gets[bk](entity)}";
                    Cache.Remove(cachekey2);
                }
                Cache.Remove(cachekey);
            }
        }

        public virtual void Remove(object key)
        {
            if (key == null || PrimaryKeyFields.Length != 1)
            {
                return;
            }
            var cachekey = $"{_KeyName}={key}";
            if (BindKeyFields.Length == 0)
            {
                Cache.Remove(cachekey);
                return;
            }
            var entity = Cache[cachekey] as T;
            Cache.Remove(cachekey);
            if (entity != null)
            {
                for (int i = 0, length = BindKeyFields.Length; i < length; i++)
                {
                    var bk = BindKeyFields[i];
                    var cachekey2 = $"{bk}={_Gets[bk](entity)}";
                    Cache.Remove(cachekey2);
                }
            }
        }

        public virtual void Remove<R>(Expression<Func<T, R>> bindKeyField, R value)
        {
            var field = Parse<R>(bindKeyField);
            var cachekey2 = $"{field}={value}";
            var key = Cache[cachekey2] as string;
            if (key == null)
            {
                return;
            }
            Cache.Remove(cachekey2);
            if (BindKeyFields.Length <= 1)
            {
                Cache.Remove($"{_KeyName}={key}");
                return;
            }
            var cachekey = $"{_KeyName}={key}";
            var entity = Cache[cachekey] as T;
            Cache.Remove(cachekey);
            for (int i = 0, length = BindKeyFields.Length; i < length; i++)
            {
                var bk = BindKeyFields[i];
                if (bk != field)
                {
                    Cache.Remove($"{bk}={_Gets[bk](entity)}");
                }
            }
        }
    }
}
