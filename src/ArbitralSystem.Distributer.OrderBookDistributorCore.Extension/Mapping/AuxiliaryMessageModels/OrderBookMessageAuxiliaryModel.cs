using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Distributer.OrderBookDistributorCore.Messaging.Mapping.AuxiliaryMessageModels
{
    internal class OrderBookMessageAuxiliaryModel : IOrderBookMessage
    {
        public Guid CorrelationId { get; set; }
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public Exchange Exchange { get; set; }
        public DateTimeOffset CatchAt { get; set; }
        public IEnumerable<OrderbookEntry> Bids { get; set; }
        public IEnumerable<OrderbookEntry> Asks { get; set; }
    }
}