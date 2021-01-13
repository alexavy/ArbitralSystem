using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributer.OrderBookDistributorCore.Messaging.Mapping.AuxiliaryMessageModels;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;

namespace ArbitralSystem.Distributer.OrderBookDistributorCore.Messaging.Mapping
{
    public class MessagesMappingProfile : Profile
    {
        public MessagesMappingProfile()
        {
            CreateMap<IOrderBook, IOrderBookMessage>().As<OrderBookMessageAuxiliaryModel>();
            CreateMap<IOrderBook, OrderBookMessageAuxiliaryModel>();
        }
    }
}