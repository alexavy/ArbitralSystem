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
        }
    }
}