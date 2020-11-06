using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using AutoMapper;
using DistributorFilter = ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models.DistributorFilter;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Mapping
{
    internal class ApiToDomainProfile : Profile
    {
        public ApiToDomainProfile()
        {
            CreateMap<DistributorFilter, ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels.DistributorFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null));
                
            CreateMap<DistributorFilter, OrderBookDistributorFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null));
            
            CreateMap<v1.Models.ServerFilter, ServerFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null));
        }
    }
}