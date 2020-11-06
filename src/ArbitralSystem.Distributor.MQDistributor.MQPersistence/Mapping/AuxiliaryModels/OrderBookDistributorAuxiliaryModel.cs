using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using ArbitralSystem.Domain.MarketInfo;
using JetBrains.Annotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Mapping.AuxiliaryModels
{
    [UsedImplicitly]
    internal class OrderBookDistributorAuxiliaryModel : IOrderBookDistributor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DistributorType Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public Status Status { get; set; }
        public IServerReference Server { get; set; }
        public IEnumerable<IExchangeInfo> ExchangeInfos { get; set; }
    }
}