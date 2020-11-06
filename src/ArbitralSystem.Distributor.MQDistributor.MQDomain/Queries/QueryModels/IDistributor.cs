using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public interface IDistributor
    {
        public Guid Id { get; }
        public string Name { get; }
        public DistributorType Type { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; }
        public Status Status { get; }
        public IServerReference Server { get; }
        public IEnumerable<IExchangeInfo> ExchangeInfos { get; }
    }
}