using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ArbitralSystem.Storage.RemoteCacheStorage
{
    public class PairCacheStorageRedisClient : IPairCacheStorage
    {
        private readonly ILogger _logger;
        private readonly IRedisWrapper _redisWrapper;

        public PairCacheStorageRedisClient(IRedisWrapper redisWrapper,
            ILogger logger)
        {
            _redisWrapper = redisWrapper;
            _logger = logger;
        }

        public async Task<IList<IPairInfo>> GetAllPairsAsync()
        {
            _logger.Verbose("Getting all pairs");
            var keys = _redisWrapper.GetKeys($"{nameof(IPairInfo)}|*");
            return await GetValuesByKeysAsync(keys);
        }

        public async Task<IList<IPairInfo>> GetAllPairsAsync(Exchange exchange)
        {
            _logger.Verbose($"Getting all pairs, for exchange: {exchange}");
            var keys = _redisWrapper.GetKeys($"{nameof(IPairInfo)}|{exchange}|*");
            return await GetValuesByKeysAsync(keys);
        }

        public async Task<bool> SetPairAsync(IPairInfo pair)
        {
            _logger.Verbose("Setting pair: {@pair}", pair);
            var key = $"{nameof(IPairInfo)}|{pair.Exchange}|{pair.UnificatedPairName}";
            return await _redisWrapper.SetObjectAsync(key, pair);
        }

        public void Dispose()
        {
            _redisWrapper.Dispose();
        }

        private async Task<IList<IPairInfo>> GetValuesByKeysAsync(IEnumerable<RedisKey> keys)
        {
            var rawValues = await _redisWrapper.GetValuesAsync(keys);
            var pairValues = new List<IPairInfo>();
            if (rawValues.Any())
            {
                foreach (var rawValue in rawValues)
                    if (rawValue.HasValue)
                    {
                        var pairInfo = JsonConvert.DeserializeObject<IPairInfo>(rawValue);

                        if (pairInfo != null)
                            pairValues.Add(pairInfo);
                    }

                ;
            }

            return pairValues;
        }
    }
}