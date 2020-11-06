using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Interfaces;
using ArbitralSystem.Messaging.Common;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributer.OrderBookDistributorCore.Messaging
{
    public class OrderBookPublisher : IOrderBookPublisher
    {
        //private readonly IDomainBusProducer _busProducer;
        private readonly IMapper _mapper;

        public OrderBookPublisher( )
        {
            //Preconditions.CheckNotNull(busProducer, mapper);
            //_busProducer = busProducer;
           // _mapper = mapper;
        }

        public  Task Publish(IOrderBook orderbook)
        {
            return Task.CompletedTask;
            //var orderBookMessage = _mapper.Map<IOrderBookMessage>(orderbook);
            //await _busProducer.PublishAsync(orderBookMessage);
        }

        public Task Publish(IDistributerState orderBookState)
        {
            return Task.CompletedTask;
            //var distributerStateMessage = _mapper.Map<IDistributerStateMessage>(orderBookState);
            //await _busProducer.PublishAsync(distributerStateMessage);
        }
    }
}