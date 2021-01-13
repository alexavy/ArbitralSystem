using System;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Mapping
{
    public class DomainToPersistenceMappingProfile : Profile
    {
        public DomainToPersistenceMappingProfile()
        {
            CreateMap<PairInfo, Entities.PairInfo>()
                .ForMember(destination => destination.UtcCreatedAt, o => o.MapFrom(source => source.CreatedAt.UtcDateTime))
                .ForMember(destination => destination.UtcDelistedAt,
                    o => o.MapFrom(source => source.DelistedAt.HasValue ? source.DelistedAt.Value.ToUniversalTime() : (DateTimeOffset?) null ));

            CreateMap<PairPrice, Entities.PairPrice>()
                .ForMember(destination => destination.UtcDate, o => o.MapFrom(source => source.CreatedAt.UtcDateTime));
        }
    }
}