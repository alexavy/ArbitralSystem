using System.Collections.Generic;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributer.PairInfoDistributerService.Test
{
    public abstract class BakedPairs
    {
        protected List<IPairInfo> binance_3_ETHBTC_ETHLTC_ETHBNB;
        protected List<IPairInfo> binance_1_ETHBTC;
        protected List<IPairInfo> binance_2_ETHBTC_ETHLTC;

        protected BakedPairs()
        {
            binance_3_ETHBTC_ETHLTC_ETHBNB = new List<IPairInfo>() {
                new PairInfo()
                {
                    BaseCurrency = "ETH", QuoteCurrency = "BTC",
                    ExchangePairName = "ETHBTC", 
                    Exchange = Exchange.Binance
                },
                new PairInfo()
                {
                    BaseCurrency = "ETH", QuoteCurrency = "LTC",
                    ExchangePairName = "ETHLTC", 
                    Exchange = Exchange.Binance
                },
                new PairInfo()
                {
                    BaseCurrency = "ETH", QuoteCurrency = "BNB",
                    ExchangePairName = "ETHBNB",
                    Exchange = Exchange.Binance
                }
             };

            binance_1_ETHBTC = new List<IPairInfo>() {
                new PairInfo()
                {
                    BaseCurrency = "ETH", QuoteCurrency = "BTC",
                    ExchangePairName = "ETHBTC", 
                    Exchange = Exchange.Bittrex
                }
            };

            binance_2_ETHBTC_ETHLTC = new List<IPairInfo>() {
                new PairInfo()
                {
                    BaseCurrency = "ETH", QuoteCurrency = "BTC",
                    ExchangePairName = "ETHBTC", 
                    Exchange = Exchange.Bittrex
                },
                new PairInfo()
                {
                    BaseCurrency = "ETH", QuoteCurrency = "LTC",
                    ExchangePairName = "ETHLTC", 
                    Exchange = Exchange.Binance
                }
            };
        }

    }
}
