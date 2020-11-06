using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using Bittrex.Net;
using Bittrex.Net.Interfaces;
using Bittrex.Net.Objects;
using JetBrains.Annotations;

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    public class BittrexPublicConnector : IPublicConnector
    {
        private readonly IBittrexClient _bittrexClient;
        private readonly IDtoConverter _converter;

        public BittrexPublicConnector([NotNull] IDtoConverter converter,
            IBittrexClient bittrexClient = null)
        {
            if (bittrexClient == null)
                _bittrexClient = new BittrexClient();
            else
                _bittrexClient = bittrexClient;

            _converter = converter;
        }

        async Task<long> IPublicConnector.GetServerTime()
        {
            var response = await _bittrexClient.GetSymbolsAsync();
            var dateTimeString = response.ResponseHeaders.FirstOrDefault(o => o.Key == "Date")
                .Value?.FirstOrDefault();

            var dateTimeUtc = DateTime.Parse(dateTimeString).ToUniversalTime();

            return TimeHelper.DateTimeToTimeStamp(dateTimeUtc);
        }

        async Task<IEnumerable<IPairInfo>> IPublicConnector.GetPairsInfo()
        {
            var response = await _bittrexClient.GetSymbolsAsync();

            return _converter.Convert<IEnumerable<BittrexSymbol>, IEnumerable<PairInfo>>(response.Data);
        }
    }
}