using System.Collections;
using System.Collections.Generic;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;
using MediatR;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Commands
{
    public class BulkSaveOrderBooksCommand : IRequest
    {
        public IEnumerable<OrderBook> OrderBooks { get; }

        public BulkSaveOrderBooksCommand(IEnumerable<OrderBook> orderBooks)
        {
            OrderBooks = orderBooks;
        }
    }
}