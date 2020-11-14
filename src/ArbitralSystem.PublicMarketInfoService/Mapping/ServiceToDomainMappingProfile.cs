using System.Collections.Generic;
using ArbitralSystem.PublicMarketInfoService.v1.Models;
using AutoMapper;


namespace ArbitralSystem.PublicMarketInfoService.Mapping
{
    internal class ServiceToDomainMappingProfile : Profile
    {
        public ServiceToDomainMappingProfile()
        {
            CreateMap<PairInfoFilter, Domain.Queries.Filters.PairInfoFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null));

            CreateMap<PairPriceFilter, Domain.Queries.Filters.IntervalPageFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null));

            CreateMap<SummaryPairPriceFilter, Domain.Queries.Filters.IntervalFilter>();
            
            CreateMap<PolygonFilter, Domain.Queries.Filters.PolygonFilter>();
        }
    }
}