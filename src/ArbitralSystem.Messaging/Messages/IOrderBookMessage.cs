using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IOrderBookMessage : ICorrelation
    {
        string Symbol { get; }
        Exchange Exchange { get; }
        DateTimeOffset CatchAt { get; }
        IEnumerable<OrderbookEntry> Bids { get; }
        IEnumerable<OrderbookEntry> Asks { get; }
    }
}