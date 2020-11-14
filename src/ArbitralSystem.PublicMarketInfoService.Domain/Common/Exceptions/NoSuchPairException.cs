using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Common.Exceptions
{
    public class NoSuchPairException : Exception
    {
        public string Pair { get; }
        public Exchange Exchange { get; }
 
        public NoSuchPairException(string pair, Exchange exchange) : base($"No such pair {pair} at {exchange.ToString()}")
        {
            Pair = pair;
            Exchange = exchange;
        }
    }
}