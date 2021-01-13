using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Storage.RemoteCacheStorage.Options;
using JetBrains.Annotations;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace ArbitralSystem.Storage.RemoteCacheStorage
{
    [Obsolete("6000 operations per hour, so use free, not limited- PairCacheStorageRedisClient")]
    public class PairCacheStorageServiceStack : IPairCacheStorage
    {
        private readonly ICacheStorageOptions _cacheStorageOptions;
        private readonly IRedisTypedClient<IPairInfo> _pairsCash;

        private readonly RedisClient _redisClient;

        public PairCacheStorageServiceStack([NotNull] ICacheStorageOptions cacheStorageOptions)
        {
            _cacheStorageOptions = cacheStorageOptions;

            _redisClient = new RedisClient(cacheStorageOptions.Host);
            _pairsCash = _redisClient.As<IPairInfo>();
        }

        public Task<bool> SetPairAsync(IPairInfo pair)
        {
            var key = $"{pair.Exchange.ToString()}:{pair.UnificatedPairName}";
            _pairsCash.SetValue(key, pair);
            var isSuccess = true;
            if (_cacheStorageOptions.ExpirationTime.HasValue)
                isSuccess = _pairsCash.ExpireEntryIn(key, _cacheStorageOptions.ExpirationTime.Value);

            return Task.FromResult(isSuccess);
        }

        public Task<IList<IPairInfo>> GetAllPairsAsync()
        {
            return Task.FromResult(_pairsCash.GetAll());
        }

        public Task<IList<IPairInfo>> GetAllPairsAsync(Exchange exchange)
        {
            var allKeys = _pairsCash.GetAllKeys();
            var pairExKeys = allKeys
                .Where(o => o.Contains(exchange.ToString()))
                .ToList();

            return Task.FromResult((IList<IPairInfo>) _pairsCash.GetValues(pairExKeys));
        }

        public void Dispose()
        {
            _redisClient.Dispose();
        }
    }
}