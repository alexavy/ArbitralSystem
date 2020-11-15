using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models
{
    public class OrderBookDistributorResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ServerName { get; set; }
        public string QueueName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string UnificatedPairName { get; set; }
        public IEnumerable<Exchange> Exchanges { get; set; }
    }
}