using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Messaging.Models
{
    public class HeartBeatOrderBookDistributor
    {
        public Guid DistributorId { get; set; }
        public Exchange Exchange { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
    }
}