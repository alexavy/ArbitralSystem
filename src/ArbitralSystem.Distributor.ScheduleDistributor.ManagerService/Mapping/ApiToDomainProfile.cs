using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models;
using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Mapping
{
    internal class ApiToDomainProfile : Profile
    {
        public ApiToDomainProfile()
        {
            CreateMap<DistributorFilter, ScheduleDistributor.Domain.Queries.QueryModels.DistributorFilter>()
                .ForMember(dest => dest.Offset, opt => opt.Condition(source => source.Offset != null))
                .ForMember(dest => dest.Count, opt => opt.Condition(source => source.Count != null));
        }
    }
}