using System;
using System.Collections.Generic;
using ArbitralSystem.Messaging.Messages;

namespace ArbitralSystem.Distributor.MarketInfoAggregatorService.Models
{
    internal class OrderBookPackageMessage : BaseMessage, IOrderBookPackageMessage
    {
        public IEnumerable<IOrderBookMessage> OrderBooks { get; }

        public OrderBookPackageMessage(IEnumerable<IOrderBookMessage> orderBooks)
        {
            OrderBooks = orderBooks;
        }
    }
}