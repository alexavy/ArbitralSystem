using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitralSystem.Storage.RemoteCacheStorage.Options
{
    public interface ICacheStorageOptions : ICloneable
    {
        string Host { get; }

        TimeSpan? ExpirationTime { get; }

        bool IsOptional { get; }
    }
}
