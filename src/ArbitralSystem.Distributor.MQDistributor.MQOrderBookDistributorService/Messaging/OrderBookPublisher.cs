using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.Core.Interfaces;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using MassTransit;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Messaging
{
    internal class OrderBookPublisher : IOrderBookPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        
        public OrderBookPublisher(IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            Preconditions.CheckNotNull(publishEndpoint, mapper);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        
        public async Task Publish(IOrderBook orderbook)
        {
            await _publishEndpoint.Publish(_mapper.Map<IOrderBookMessage>(orderbook));
        }

        public async Task Publish(IDistributerState orderBookState)
        {
            await _publishEndpoint.Publish(_mapper.Map<IDistributerStateMessage>(orderBookState));
        }
    }
}