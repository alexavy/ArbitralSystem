using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorService.v1.Models
{
    public class OrderBookDistributorData
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
    }
}