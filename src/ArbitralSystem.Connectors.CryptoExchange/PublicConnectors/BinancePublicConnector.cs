﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot.MarketData;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("ArbitralSystem.Connectors.Integration.Test.SingleOrderBookDistibuter")]

namespace ArbitralSystem.Connectors.CryptoExchange.PublicConnectors
{
    internal class BinancePublicConnector : BasePublicConnector, IPublicConnector
    {
        private readonly IBinanceClient _binanceClient;
        private readonly IDtoConverter _converter;

        public BinancePublicConnector([NotNull] IDtoConverter converter,
            IBinanceClient binanceClient = null)
        {
            if (binanceClient == null)
                _binanceClient = new BinanceClient();
            else
                _binanceClient = binanceClient;

            _converter = converter;
        }

        async Task<long> IPublicConnector.GetServerTime(CancellationToken ct)
        {
            var response = await _binanceClient.GetServerTimeAsync(ct:ct);
            ValidateResponse(response);
            return TimeHelper.DateTimeToTimeStamp(response.Data);
        }

        async Task<IEnumerable<IPairInfo>> IPublicConnector.GetPairsInfo(CancellationToken ct)
        {
            var response = await _binanceClient.GetExchangeInfoAsync(ct);
            ValidateResponse(response);
            return _converter.Convert<IEnumerable<BinanceSymbol>, IEnumerable<PairInfo>>(response.Data.Symbols);
        }

        public async Task<IEnumerable<IPairPrice>> GetPairPrices(CancellationToken ct)
        {
            var response = await _binanceClient.GetAllPricesAsync(ct);
            ValidateResponse(response);
            return _converter.Convert<IEnumerable<BinancePrice>, IEnumerable<PairPrice>>(response.Data);
        }
    }
}