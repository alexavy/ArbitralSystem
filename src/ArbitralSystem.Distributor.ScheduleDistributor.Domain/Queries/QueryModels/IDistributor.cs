using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels
{
    public interface IDistributor
    {
        public string Id { get; }
        public string Name { get; }
        public string DistributorType { get; }
        public string ServerName { get; }
        public string QueueName { get; }
        public DateTimeOffset CreatedAt { get; }
    }
    
    public interface IOrderBookDistributor:  IDistributor
    {    
        public string UnificatedPairName { get; }
        IEnumerable<Exchange> Exchanges { get; }
    }
}