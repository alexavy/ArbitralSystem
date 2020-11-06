using ArbitralSystem.Common.Helpers;
using ArbitralSystem.Connectors.CoinEx.Models;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Connectors.CryptoExchange.Models;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;
using AutoMapper;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot.MarketData;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using Huobi.Net.Objects;
using Kraken.Net.Objects;
using Kucoin.Net.Objects;

namespace ArbitralSystem.Connectors.CryptoExchange.Converter
{
    public class CryptoExchangeConverter : IDtoConverter
    {
        private readonly IMapper _mapper;

        public CryptoExchangeConverter()
        {
            var config = CreateMappingConfiguration();
            _mapper = config.CreateMapper();
        }

        public TDestination Convert<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        private MapperConfiguration CreateMappingConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderBookStatus, DistributerSyncStatus>();

                cfg.CreateMap<ISymbolOrderBookEntry, IOrderbookEntry>().As<OrderbookEntry>();
                cfg.CreateMap<ISymbolOrderBookEntry, OrderbookEntry>();
                cfg.CreateMap<SymbolOrderBook, OrderBook>()
                    .ForMember(destination => destination.CatchAt, o => o.MapFrom(source => source.LastOrderBookUpdate.ToUniversalTime())); // LastOrderBookUpdate is utc date
                    //.AfterMap((src, dest) => dest.TimeStamp = TimeHelper.DateTimeToTimeStamp(dest.DateTime));

                //Binance Symbol CryptoExchangeLibrary
                cfg.CreateMap<BinanceSymbol, PairInfo>()
                    .ForMember(destination => destination.ExchangePairName, o => o.MapFrom(source => source.Name))
                    .ForMember(destination => destination.BaseCurrency, o => o.MapFrom(source => source.BaseAsset))
                    .ForMember(destination => destination.QuoteCurrency, o => o.MapFrom(source => source.QuoteAsset))
                    .AfterMap((src, dest) => dest.Exchange = Exchange.Binance);

                //Bittrex Symbol CryptoExchangeLibrary
                cfg.CreateMap<BittrexSymbol, PairInfo>()
                    .ForMember(destination => destination.ExchangePairName, o => o.MapFrom(source => source.Symbol))
                    .AfterMap((src, dest) => dest.Exchange = Exchange.Bittrex);

                //Huobi Symbol CryptoExchangeLibrary
                cfg.CreateMap<HuobiSymbol, PairInfo>()
                    .ForMember(destination => destination.ExchangePairName, o => o.MapFrom(source => source.Symbol))
                    .AfterMap((src, dest) => dest.Exchange = Exchange.Huobi);

                //Kraken Symbol CryptoExchangeLibrary
                cfg.CreateMap<KrakenSymbol, PairInfo>()
                    .ForMember(destination => destination.BaseCurrency, o => o.MapFrom(source => source.BaseAsset))
                    .ForMember(destination => destination.QuoteCurrency, o => o.MapFrom(source => source.QuoteAsset))
                    .ForMember(destination => destination.ExchangePairName,
                        o => o.MapFrom(source => source.WebsocketName))
                    .AfterMap((src, dest) => dest.Exchange = Exchange.Kraken);

                //Kucoin Symbol CryptoExchangeLibrary
                cfg.CreateMap<KucoinSymbol, PairInfo>()
                    .ForMember(destination => destination.ExchangePairName, o => o.MapFrom(source => source.Symbol))
                    .AfterMap((src, dest) => dest.Exchange = Exchange.Kucoin);

                //CoinEx
                cfg.CreateMap<MarketInfo, PairInfo>()
                    .ForMember(destination => destination.ExchangePairName, o => o.MapFrom(source => source.Name))
                    .ForMember(destination => destination.BaseCurrency, o => o.MapFrom(source => source.TradingName))
                    .ForMember(destination => destination.QuoteCurrency, o => o.MapFrom(source => source.PricingName))
                    .AfterMap((src, dest) => dest.Exchange = Exchange.CoinEx);
            });
        }
    }
}