using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.Caching;

namespace NetMemCache
{
    public class MemCache<TKey, T> : MemoryCache
    {
        private readonly Func<T, TKey> _keySelector;
        private readonly CacheItemPolicy _uniformPolicy;

        public MemCache(Func<T, TKey> keySelector,
            MemCacheSettings settings = null) : base(nameof(T),
            new NameValueCollection()
            {
                ["cacheMemoryLimitMegabytes"] = SettingsOrDefault(settings).MemoryLimitMegabytes.ToString(),
                ["physicalMemoryLimitPercentage"] = SettingsOrDefault(settings).PhysicalMemoryLimitPercentage.ToString(),
                ["pollingInterval"] = SettingsOrDefault(settings).PollingInterval.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture),
            })
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            _keySelector = keySelector;

            settings = SettingsOrDefault(settings);

            _uniformPolicy = new CacheItemPolicy() { SlidingExpiration = settings.SlidingExpiration };
        }

        public void AddItem(T item)
        {
            if (item != null)
            {
                var key = GetKey(item);
                if (key != null)
                    Set(key, item, _uniformPolicy);
            }
        }

        public void RemoveItem(T item)
        {
            if (item != null)
            {
                var key = GetKey(item);
                if (key != null)
                    Remove(key);
            }
        }

        public T Retrieve(TKey key)
        {
            if (key == null) return default(T);
            return (T)Get(key.ToString());
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null) return false;
            return Contains(key.ToString());
        }

        private string GetKey(T item)
        {
            return item == null ? null : _keySelector(item).ToString();
        }

        private static MemCacheSettings SettingsOrDefault(MemCacheSettings settings)
        {
            return settings ?? MemCacheSettings.Default();
        }
    }
}
