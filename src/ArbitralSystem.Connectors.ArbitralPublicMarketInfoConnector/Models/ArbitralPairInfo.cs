using System;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.ArbitralPublicMarketInfoConnector.Models
{
    public class ArbitralPairInfo : IArbitralPairInfo
    {
        public Exchange Exchange { get;  set; }
        public string UnificatedPairName { get; set; }
        public string ExchangePairName { get;set; }
        public string BaseCurrency { get; set;}
        public string QuoteCurrency { get; set;}
        public DateTimeOffset CreatedAt { get; set;}
        public DateTimeOffset? DelistedAt { get; set; }
    }
}