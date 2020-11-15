using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.v1.Models.Paging;
using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Mapping
{
    internal class DomainToApiProfile : Profile
    {
        public DomainToApiProfile()
        {
            CreateMap(typeof(ArbitralSystem.Common.Pagination.Page<>), typeof(Page<>));
            CreateMap<IOrderBookDistributor, OrderBookDistributorResult>();
        }
    }
}