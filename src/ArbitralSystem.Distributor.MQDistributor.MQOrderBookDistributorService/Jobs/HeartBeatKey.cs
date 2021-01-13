using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Jobs
{
    internal struct HeartBeatKey
    {
        public HeartBeatKey(Guid distributorId, Exchange exchange)
        {
            DistributorId = distributorId;
            Exchange = exchange;
        }

        public Guid DistributorId {get;}
        public Exchange Exchange { get; }
    }
}