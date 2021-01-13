using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArbitralSystem.Storage.RemoteCacheStorage
{
    public interface IRedisWrapper: IDisposable
    {
        IEnumerable<RedisKey> GetKeys(string pattern);

        Task<IEnumerable<RedisValue>> GetValuesAsync(IEnumerable<RedisKey> redisKeys);

        Task<bool> SetObjectAsync(string key, object obj);
    }
}
