using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Messaging
{
    internal class OrderBookMessage : BaseMessage, IOrderBookMessage
    {
        public string Symbol { get; set;}
        public Exchange Exchange { get; set;}
        public DateTimeOffset CatchAt { get; set;}
        public IEnumerable<OrderbookEntry> Bids { get; set;}
        public IEnumerable<OrderbookEntry> Asks { get; set;}
    }
}