using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;
using MassTransit;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Messaging
{
    internal class OrderBookPublisher : IOrderBookPublisher
    {
        private readonly IBusControl _busControl;
        private readonly IMapper _mapper;
        
        public OrderBookPublisher(IBusControl busControl, IMapper mapper)
        {
            Preconditions.CheckNotNull(busControl, mapper);
            _mapper = mapper;
            _busControl = busControl;
        }
        
        public async Task Publish(IOrderBook orderbook)
        {
            await _busControl.Publish(_mapper.Map<IOrderBookMessage>(orderbook));
        }

        public async Task Publish(IDistributerState orderBookState)
        {
            await _busControl.Publish(_mapper.Map<IDistributerStateMessage>(orderBookState));
        }
    }
}