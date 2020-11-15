using System;

namespace ArbitralSystem.Storage.RemoteCacheStorage.Options
{
    public class CacheStorageOptions : ICacheStorageOptions
    {
        public string Host { get; set; }

        public TimeSpan? ExpirationTime { get; set; }

        public bool IsOptional { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
