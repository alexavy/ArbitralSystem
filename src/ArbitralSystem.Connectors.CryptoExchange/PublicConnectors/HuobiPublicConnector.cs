using System.Collections.Generic;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using Huobi.Net;
using Huobi.Net.Interfaces;
using Huobi.Net.Objects;
using JetBrains.Annotations;

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    public class HuobiPublicConnector : IPublicConnector
    {
        private readonly IDtoConverter _converter;
        private readonly IHuobiClient _huobiClient;

        public HuobiPublicConnector([NotNull] IDtoConverter converter,
            IHuobiClient huobiClient = null)
        {
            if (huobiClient == null)
                _huobiClient = new HuobiClient();
            else
                _huobiClient = huobiClient;

            _converter = converter;
        }

        async Task<long> IPublicConnector.GetServerTime()
        {
            var response = await _huobiClient.GetServerTimeAsync();
            return TimeHelper.DateTimeToTimeStamp(response.Data.ToUniversalTime());
        }

        async Task<IEnumerable<IPairInfo>> IPublicConnector.GetPairsInfo()
        {
            var response = await _huobiClient.GetSymbolsAsync();
            return _converter.Convert<IEnumerable<HuobiSymbol>, IEnumerable<PairInfo>>(response.Data);
        }
    }
}