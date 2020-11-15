using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping.AuxiliaryModels
{
    public class OrderBookDistributorAuxiliaryModel : IOrderBookDistributor
    {
        public string Id { get; }
        public string Name { get; }
        public string DistributorType { get; }
        public string ServerName { get; }
        public string QueueName { get; }
        public DateTimeOffset CreatedAt { get; }
        public string UnificatedPairName { get; }
        public IEnumerable<Exchange> Exchanges { get; }

        public OrderBookDistributorAuxiliaryModel(IDistributor distributor, string unificatedPairName, IEnumerable<Exchange> exchanges)
        {
            Id = distributor.Id;
            Name = distributor.Name;
            DistributorType = distributor.DistributorType;
            ServerName = distributor.ServerName;
            QueueName = distributor.QueueName;
            CreatedAt = distributor.CreatedAt;
            UnificatedPairName = unificatedPairName;
            Exchanges = exchanges;
        }
    }
}