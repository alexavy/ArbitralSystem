using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.CoinEx.Models;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Models;

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    public class CoinExPublicConnector : IPublicConnector
    {
        private readonly ICoinExConnector _coinExConnector;
        private readonly IDtoConverter _converter;
        private readonly ILogger _logger;

        public CoinExPublicConnector(ICoinExConnector coinExConnector,
            IDtoConverter converter,
            ILogger logger)
        {
            _coinExConnector = coinExConnector;
            _converter = converter;
            _logger = logger;
        }

        public Task<long> GetServerTime()
        {
            throw new NotSupportedException("CoinEx server time not supported");
        }

        public async Task<IEnumerable<IPairInfo>> GetPairsInfo()
        {
            var pairs = await _coinExConnector.GetMarketList();
            var pairsForResult = new List<PairInfo>();

            if (pairs.IsSuccess)
            {
                _logger.Debug("List of pairs for CoinEx successfully received");
                foreach (var pair in pairs.Data)
                    if (!pair.Any(o => char.IsDigit(o)))
                    {
                        var rawPairInfo = await _coinExConnector.GetMarketSingleInfo(pair);
                        if (rawPairInfo.IsSuccess)
                        {
                            _logger.Debug("Pair information received: {@rawPairInfo}", rawPairInfo.Data);
                            pairsForResult.Add(_converter.Convert<MarketInfo, PairInfo>(rawPairInfo.Data));
                        }
                    }
            }
            else
            {
                throw pairs.Exception;
            }
            return pairsForResult;
        }
    }
}