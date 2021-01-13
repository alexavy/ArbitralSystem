using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public interface IExchangeInfo
    {
        public Exchange Exchange { get; }
        public DateTimeOffset? HeartBeat { get; }
    }
}