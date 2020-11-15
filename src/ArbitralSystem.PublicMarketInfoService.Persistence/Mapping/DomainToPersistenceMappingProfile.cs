using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Mapping
{
    public class DomainToPersistenceMappingProfile : Profile
    {
        public DomainToPersistenceMappingProfile()
        {
            CreateMap<PairInfo, Entities.PairInfo>();
            CreateMap<PairPrice, Entities.PairPrice>()
                .ForMember(destination => destination.Date, o => o.MapFrom(source => source.CreatedAt));
        }
    }
}