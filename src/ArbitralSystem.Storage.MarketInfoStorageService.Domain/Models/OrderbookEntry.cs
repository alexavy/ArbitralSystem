using System;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models
{
    public class OrderbookEntry
    {
        public decimal Quantity { get; }
        public decimal Price { get; }

        public OrderbookEntry(decimal price, decimal quantity)
        {
            //Price can be zero.
            //if(price == 0)
              //  throw new ArgumentException("Price can not be zero");
            Price = price;
            
            if(quantity == 0)
                throw new ArgumentException("Price can not be zero");
            Quantity = quantity;
        }
    }
}