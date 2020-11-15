using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using ArbitralSystem.PublicMarketInfoService.Persistence.Mapping.AuxiliaryModels;
using ArbitralSystem.PublicMarketInfoService.Persistence.Queries;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Mapping
{
    public class PersistenceToDomainMappingProfile : Profile
    {
        public PersistenceToDomainMappingProfile()
        {
            CreateMap<Entities.PairInfo, PairInfo>().ConstructUsing((o, ctx) =>
                new PairInfo(o.Id, o.ExchangePairName, o.UnificatedPairName, o.BaseCurrency, o.QuoteCurrency, o.CreatedAt, o.DelistedAt, o.Exchange));
            
            CreateMap<Entities.PairInfo, IPairInfo>().As<PairInfoAuxiliaryModel>();
            CreateMap<Entities.PairInfo, PairInfoAuxiliaryModel>();
        }
    }
}