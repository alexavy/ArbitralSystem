using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using JetBrains.Annotations;
using Kucoin.Net;
using Kucoin.Net.Interfaces;
using Kucoin.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    public class KucoinPublicConnector : IPublicConnector
    {
        private readonly IDtoConverter _converter;
        private readonly IKucoinClient _kucoinClient;

        public KucoinPublicConnector([NotNull] IDtoConverter converter,
            IKucoinClient kucoinClient = null)
        {
            if (kucoinClient == null)
                _kucoinClient = new KucoinClient();
            else
                _kucoinClient = kucoinClient;

            _converter = converter;
        }

        async Task<long> IPublicConnector.GetServerTime()
        {
            var response = await _kucoinClient.GetServerTimeAsync();
            return TimeHelper.DateTimeToTimeStamp(response.Data.ToUniversalTime());
        }

        async Task<IEnumerable<IPairInfo>> IPublicConnector.GetPairsInfo()
        {
            var response = await _kucoinClient.GetSymbolsAsync();
            return _converter.Convert<IEnumerable<KucoinSymbol>, IEnumerable<PairInfo>>(response.Data);
        }
    }
}