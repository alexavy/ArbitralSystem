using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Mapping
{
    internal class ConnectorsToDomainProfile : Profile 
    {
        public ConnectorsToDomainProfile()
        {
            CreateMap<IArbitralPairInfo, PairInfo>()
                .ConstructUsing( o=> new PairInfo(o.Exchange,o.UnificatedPairName,o.ExchangePairName));
        }
    }
}