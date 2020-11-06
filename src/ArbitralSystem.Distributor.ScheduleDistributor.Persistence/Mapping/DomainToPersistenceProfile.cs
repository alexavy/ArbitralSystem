using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping
{
    public class DomainToPersistenceProfile : Profile
    {
        public DomainToPersistenceProfile()
        {
            CreateMap<ScheduleDistributor.Domain.Models.Distributor, Entities.Distributor>();
        }
    }
}