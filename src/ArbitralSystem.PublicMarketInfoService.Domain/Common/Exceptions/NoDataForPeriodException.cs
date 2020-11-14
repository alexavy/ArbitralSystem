using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Common.Exceptions
{
    public class NoDataForPeriodException : Exception
    {
        public string Pair { get; }
        public Exchange Exchange { get; }
 
        public NoDataForPeriodException(string pair, Exchange exchange) : base($"No prices for {pair} at {exchange.ToString()} at Such period")
        {
            Pair = pair;
            Exchange = exchange;
        }
    }
}