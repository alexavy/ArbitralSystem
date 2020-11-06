using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using JetBrains.Annotations;
using Kraken.Net;
using Kraken.Net.Interfaces;
using Kraken.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    public class KrakenPublicConnector : IPublicConnector
    {
        private readonly IDtoConverter _converter;
        private readonly IKrakenClient _krakenClient;

        public KrakenPublicConnector([NotNull] IDtoConverter converter,
            IKrakenClient krakenClient = null)
        {
            if (krakenClient == null)
                _krakenClient = new KrakenClient();
            else
                _krakenClient = krakenClient;

            _converter = converter;
        }

        async Task<long> IPublicConnector.GetServerTime()
        {
            var response = await _krakenClient.GetServerTimeAsync();
            return TimeHelper.DateTimeToTimeStamp(response.Data.ToUniversalTime());
        }

        async Task<IEnumerable<IPairInfo>> IPublicConnector.GetPairsInfo()
        {
            var response = await _krakenClient.GetSymbolsAsync();
            return _converter.Convert<IEnumerable<KrakenSymbol>, IEnumerable<PairInfo>>(response.Data.Values);
        }
    }
}