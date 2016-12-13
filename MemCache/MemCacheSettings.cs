using System;

namespace NetMemCache
{
    public class MemCacheSettings
    {
        public int MemoryLimitMegabytes { get; set; }
        public int PhysicalMemoryLimitPercentage { get; set; }
        public TimeSpan PollingInterval { get; set; }
        public TimeSpan SlidingExpiration { get; set; }

        public static MemCacheSettings Default()
        {
            return new MemCacheSettings()
            {
                MemoryLimitMegabytes = 100,
                PhysicalMemoryLimitPercentage = 10,
                PollingInterval = TimeSpan.FromMinutes(1),
                SlidingExpiration = TimeSpan.FromHours(1),
            };
        }
    }
}