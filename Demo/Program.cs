using blqw.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User { ID = 1, Name = "blqw", LoginName = "blqw1" };
            var user2 = new User { ID = 2, Name = "blqw", LoginName = "blqw2" };
            var user3 = new User { ID = 3, Name = "blqw", LoginName = "blqw3" };
            var user4 = new User { ID = 4, Name = "blqw", LoginName = "blqw4" };
            var user5 = new User { ID = 5, Name = "blqw", LoginName = "blqw5" };

            var db = CacheStore<User>.Instance;
            db.Add(user1);
            db.Add(user2);
            db.Add(user3);
            db.Add(user4);
            db.Add(user5);

            var u1 = db.Get(1);
            var u2 = db.Get(it => it.LoginName, "blqw2");
            db.Remove(it => it.LoginName, "blqw3");
            var u3 = db.Get(3);
            db.Remove(4);
            var u4 = db.Get(4);

        }

    }


    public class User
    {
        [CacheKey]
        public int ID { get; set; }
        public string Name { get; set; }
        [BindKey]
        public string LoginName { get; set; }
    }

}
