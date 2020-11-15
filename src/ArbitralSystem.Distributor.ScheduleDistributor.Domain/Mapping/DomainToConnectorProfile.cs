using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Mapping
{
    public class DomainToConnectorProfile : Profile
    {
        public DomainToConnectorProfile()
        {
            CreateMap<PairInfo, OrderBookPairInfo>().ConstructUsing(o => new OrderBookPairInfo(o.Exchange, o.ExchangePairName, o.UnificatedPairName));
        }
    }
}