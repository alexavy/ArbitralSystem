using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Common
{
    public class DistributorHeartBeat
    {
        public DistributorHeartBeat(Guid distributorId, Exchange exchange, DateTimeOffset dateTimeOffset)
        {
            DistributorId = distributorId;
            Exchange = exchange;
            DateTimeOffset = dateTimeOffset;
        }

        public Guid DistributorId { get; }
        public Exchange Exchange { get; }
        public DateTimeOffset DateTimeOffset { get; }
    }
}