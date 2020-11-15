using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Messaging;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;
using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Mapping
{
    internal class DomainToMessaging : Profile
    {
        public DomainToMessaging()
        {
            CreateMap<IOrderBook,IOrderBookMessage>().As<OrderBookMessage>();
            CreateMap<IOrderBook,OrderBookMessage>();
            CreateMap<IOrderbookEntry, OrderbookEntry>();
            
            CreateMap<IDistributerState,IDistributerStateMessage>().As<DistributerStateMessage>();
            CreateMap<IDistributerState, DistributerStateMessage>();
        }
    }
}