using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace blqw
{
    public class CacheDataBase<T> : ICacheDataBase<T>
    {
        private readonly static CacheDataBase<T> Instance = OnSingleton();

        private static CacheDataBase<T> OnSingleton()
        {

            return new CacheDataBase<T>();
        }

        protected CacheDataBase() { }

        public ObjectCache Cache { get; set; }

        public TimeSpan ExpirationTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string[] PrimaryKeyFields
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string[] UniqueKeyFields
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public T Get(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public T Get(object key)
        {
            throw new NotImplementedException();
        }

        public T Get(Expression<Func<T, object>> uniqueKeyField, object uniqueValue)
        {
            throw new NotImplementedException();
        }

        public void Remove(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<T, object>> uniqueKeyField, object uniqueValue)
        {
            throw new NotImplementedException();
        }
    }
}
