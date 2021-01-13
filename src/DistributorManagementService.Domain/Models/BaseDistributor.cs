using System;
using System.Collections.Generic;
using System.Linq;
using ArbitralSystem.Domain.Distributers;
using ArbitralSystem.Domain.MarketInfo;

namespace DistributorManagementService.Domain.Models
{
    public interface IDistributor
    {
        Guid Id { get; }
        string Name { get; }
        DistributorState DistributorState { get; }
        DateTimeOffset CreatedAt { get;  }
        DateTimeOffset? DeletedAt { get; }
        void ChangeState(DistributorState distributorState);
        void SetAsDeleted();
    }
    
    public abstract class BaseDistributor : IDistributor
    {
        public Guid Id { get; }
        public string Name { get; }
        public DistributorState DistributorState { get; private set; }
        public DateTimeOffset CreatedAt { get;  }
        public DateTimeOffset? DeletedAt { get; private set; }
        public BaseDistributor(Guid id, string name)
        {
            Id = id;
            
            if(string.IsNullOrEmpty(name) )
                throw new ArgumentException("Bot name cannot be empty");
            
            Name = name;
            DistributorState = DistributorState.Initialization;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public void ChangeState(DistributorState distributorState)
        {
            if(distributorState == DistributorState.Initialization)
                throw new ArgumentException($"Can not set {DistributorState.Initialization}, it is only for initialization bot");
            DistributorState = distributorState;
        }
        
        public void SetAsDeleted()
        {
            if(DeletedAt.HasValue)
                throw new InvalidOperationException($"Can not set bot: {Id} as deleted, its already deleted at {DeletedAt}");
            
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }

    public interface IOrderBookDistributor : IDistributor
    {
        string UnificatedPair { get; }
        IEnumerable<DistributorExchangeProperty> DistributorProperties { get; }

        void AppointPair(string unificatedPair, DistributorExchangeProperty[] distributorProperties);
    }
    
    public class OrderBookDistributor : BaseDistributor , IOrderBookDistributor
    {
        public string UnificatedPair { get; private set; }
        public IEnumerable<DistributorExchangeProperty> DistributorProperties { get; private set; }
        
        
        public OrderBookDistributor(Guid id, string name ) : base(id, name)
        {
        }

        public void AppointPair(string unificatedPair, DistributorExchangeProperty[] distributorProperties)
        {
            if(DistributorState != DistributorState.Listening)
                throw new InvalidOperationException($"Can not set pair to bot in state {DistributorState}"); 
            
            if(string.IsNullOrEmpty(unificatedPair))
                throw new ArgumentException("Unificated pair cannot be empty");
            
            UnificatedPair = unificatedPair;
            if(!distributorProperties.Any())
                throw new ArgumentException("Distributor properties can not be empty");
            DistributorProperties = distributorProperties;
        }

        public void RemovePair()
        {
            if(DistributorState != DistributorState.Distribution)
                throw new InvalidOperationException($"Can not remove pair to bot in state {DistributorState}");
            
            UnificatedPair = null;
            DistributorProperties = null;
        }
        
    }

    public class DistributorExchangeProperty
    {
        public string ExchangePairName { get; }
        public Exchange Exchange { get; }

        public DistributorExchangeProperty(string exchangePairName, Exchange exchange)
        {
            if(string.IsNullOrEmpty(exchangePairName))
                throw new ArgumentException("Exchange pair name can not be empty");

            ExchangePairName = exchangePairName;
            Exchange = exchange.ThrowIfUndefined();
        }
    }
}