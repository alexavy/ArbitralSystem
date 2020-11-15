using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;
using JetBrains.Annotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Messaging
{
    [UsedImplicitly]
    internal class DistributerStateMessage : BaseMessage, IDistributerStateMessage
    {
        public string Symbol { get; set;}
        public Exchange Exchange { get; set;}
        public DateTimeOffset ChangedAt { get; set; }
        public DistributerSyncStatus PreviousStatus { get; set;}
        public DistributerSyncStatus CurrentStatus { get; set;}
    }
    
    [UsedImplicitly]
    internal class HeartBeatOrderBookDistributorMessage : BaseMessage, IHeartBeatOrderBookDistributorMessage
    {
        public HeartBeatOrderBookDistributorMessage(IEnumerable<HeartBeatOrderBookDistributor> heartBeatBatch)
        {
            HeartBeatBatch = heartBeatBatch;
        }
        
        public IEnumerable<HeartBeatOrderBookDistributor> HeartBeatBatch { get; }
    }
    
    
}