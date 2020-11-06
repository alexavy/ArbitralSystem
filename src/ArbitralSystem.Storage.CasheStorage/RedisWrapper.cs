using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Storage.RemoteCacheStorage.Options;

namespace ArbitralSystem.Storage.RemoteCacheStorage
{
    public class RedisWrapper : IRedisWrapper
    {
        private readonly ConnectionMultiplexer _redis;

        private readonly ICacheStorageOptions _cacheStorageOptions;
        private readonly ILogger _logger;

        public RedisWrapper(ICacheStorageOptions cacheStorageOptions,
                            ILogger logger)
        {
            _cacheStorageOptions = cacheStorageOptions;
            _logger = logger;

            try
            {
                _redis = ConnectionMultiplexer.Connect(cacheStorageOptions.Host+ ",syncTimeout=10000");
                _logger.Debug($"connected to redis server, host: {cacheStorageOptions.Host}");
            }
            catch (RedisConnectionException)
            {
                if (!_cacheStorageOptions.IsOptional)
                    throw;

                _logger.Warning($"Not connected to redis server, redis now is optional");
            }
        }

        public IEnumerable<RedisKey> GetKeys(string pattern)
        {
            if(_redis == null)
            {
                return new List<RedisKey>();
            }
            else
            {
                var server = _redis.GetServer(_cacheStorageOptions.Host);
                return server.Keys(pattern: pattern);
            }
        }

        public async Task<IEnumerable<RedisValue>> GetValuesAsync(IEnumerable<RedisKey> redisKeys)
        {
            if (_redis == null)
            {
                return new List<RedisValue>();
            }
            else
            {
                var database = _redis.GetDatabase();
                var rawValues = await database.StringGetAsync(redisKeys.ToArray());
                return rawValues.ToList();
            }
        }

        public async Task<bool> SetObjectAsync(string key, object obj)
        {
            if (_redis == null)
            {
                return false;
            }
            else
            {
                var database = _redis.GetDatabase();
                var deserializedObject = JsonConvert.SerializeObject(obj);
                bool isSuccess = await database.StringSetAsync(key, deserializedObject);

                if (!isSuccess)
                {
                    _logger.Error($"Error while setting object to redis: key {key}, object: {deserializedObject}");
                    return false;
                }

                if (_cacheStorageOptions.ExpirationTime.HasValue)
                    await database.KeyExpireAsync(key, _cacheStorageOptions.ExpirationTime);

                return true;
            }
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}
