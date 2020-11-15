using System;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Messages;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Messaging
{
    internal class DistributerStateMessage : BaseMessage, IDistributerStateMessage
    {
        public string Symbol { get; set;}
        public Exchange Exchange { get; set;}
        public DateTimeOffset ChangedAt { get; set; }
        public DistributerSyncStatus PreviousStatus { get; set;}
        public DistributerSyncStatus CurrentStatus { get; set;}
    }
}