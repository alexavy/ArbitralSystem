using System;
using System.Collections.Generic;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;

namespace DistributorManagementService.v1.Models
{
    public class OrderBookDistributor
    {
        public Guid Id { get; set; }
        
        public DistributorState DistributorState { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset? DeletedAt { get; set; }
        
        public string Name { get; set; }
        
        public string UnificatedPair { get; set; }
        
        public IEnumerable<OrderBookDistributorProperty> OrderBookDistributorProperties { get; set; } 
    }

    public class OrderBookDistributorProperty
    {
        public string ExchangePairName { get; set; }
        
        public Exchange Exchange { get; set; }
    }
    
    public class OrderBookDistributorFilter
    {
        public string Name { get; set; }
        
        public string UnificatedPair { get; set; }
        
        public bool? IsDeleted { get; set; }
    }
}