using NUnit.Framework;

namespace NetMemCache.Test
{
    public class MyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [TestFixture]
    public class MemCacheTest
    {
        [Test]
        public void TestDefaultInterval()
        {
            using (var cache = CreateDefaultCache())
            {
                Assert.AreEqual(1, cache.PollingInterval.TotalMinutes);
            }
        }

        [Test]
        public void TestDefaultAbsoluteMemory()
        {
            using (var cache = CreateDefaultCache())
            {
                Assert.AreEqual(100 * 1024 * 1024, cache.CacheMemoryLimit);
            }
        }

        [Test]
        public void TestDefaultRelativeMemory()
        {
            using (var cache = CreateDefaultCache())
            {
                Assert.AreEqual(10, cache.PhysicalMemoryLimit);
            }
        }

        [Test]
        public void TestRetrieve()
        {
            using (var cache = CreateDefaultCache())
            {
                var item = new MyClass() { Id = 1, Name = "John Smith" };

                cache.AddItem(item);

                var cached = cache.Retrieve(item.Id);
                Assert.AreEqual(item.Name, cached?.Name);
            }
        }

        [Test]
        public void TestContains()
        {
            using (var cache = CreateDefaultCache())
            {
                var item = new MyClass() { Id = 1, Name = "John Smith" };

                cache.AddItem(item);

                Assert.IsTrue(cache.ContainsKey(item.Id));
            }
        }

        [Test]
        public void TestRemove()
        {
            using (var cache = CreateDefaultCache())
            {
                var item = new MyClass() { Id = 1, Name = "John Smith" };

                cache.AddItem(item);
                cache.RemoveItem(item);

                var cached = cache.Retrieve(item.Id);
                Assert.IsNull(cached);
            }
        }

        private MemCache<int, MyClass> CreateDefaultCache()
        {
            return new MemCache<int, MyClass>(x => x.Id);
        }
    }
}
