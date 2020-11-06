using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models
{
    public class OrderBookDistributorArg
    {
        public string UnificatedExchangePairName { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
    }
}