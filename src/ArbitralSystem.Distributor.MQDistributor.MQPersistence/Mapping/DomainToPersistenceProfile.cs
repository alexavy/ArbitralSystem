using AutoMapper;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Mapping
{
    public class DomainToPersistenceProfile : Profile
    {
        public DomainToPersistenceProfile()
        {
            CreateMap<MQDomain.Models.Distributor, Entities.Distributor>()
                .ForMember(x => x.Exchanges, opt => opt.Ignore());
            CreateMap<MQDomain.Models.Server, Entities.Server>()
                .ForMember(destination => destination.MaxWorkersCount, o => o.MapFrom(source => source.MaxWorkers));
        }
    }
}