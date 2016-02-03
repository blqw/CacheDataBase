using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Caching;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        class User
        {
            [CacheKey]
            public int ID { get; set; }
            [CacheKey]
            public int ID2 { get; set; }
            public string Name { get; set; }
            [BindKey]
            public string LoginName { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var user1 = new User { ID = 1, ID2 = 1, Name = "blqw", LoginName = "blqw1" };
            var user2 = new User { ID = 2, ID2 = 2, Name = "blqw", LoginName = "blqw2" };
            var user3 = new User { ID = 3, ID2 = 3, Name = "blqw", LoginName = "blqw3" };
            var user4 = new User { ID = 4, ID2 = 4, Name = "blqw", LoginName = "blqw4" };

            var db = CacheStore<User>.Instance;
            db.Add(user1);
            db.Add(user2);
            db.Add(user3);
            db.Add(user4);

            var u1 = db.Get(1, 1);
            Assert.AreEqual(u1, user1);
            var u2 = db.Get(it => it.LoginName, "blqw2");
            Assert.AreEqual(u2, user2);
            db.Remove(it => it.LoginName, "blqw3");
            var u3 = db.Get(3, 3);
            Assert.IsNull(u3);
            db.Remove(4, 4);
            var u4 = db.Get(4, 4);
            Assert.IsNull(u4);

        }
    }
}
