using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.Core.Models;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Domain.MarketInfo;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands
{
    public class CreateOrderBookDistributorCommand : IRequest<Guid>
    {
        public ExchangePairInfo ExchangePairInfo { get; }

        public CreateOrderBookDistributorCommand(ExchangePairInfo exchangePairInfo)
        {
            ExchangePairInfo = exchangePairInfo;
        }
    }
    
    public class OrderBookDistributorHeartBeatCommand : IRequest
    {
        public OrderBookDistributorHeartBeatCommand(IEnumerable<DistributorHeartBeat> heartBeats)
        {
            HeartBeats = heartBeats;
        }

        public IEnumerable<DistributorHeartBeat> HeartBeats { get; }
    }
}