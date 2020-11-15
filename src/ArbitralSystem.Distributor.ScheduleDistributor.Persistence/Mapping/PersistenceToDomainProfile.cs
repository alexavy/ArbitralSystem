using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping.AuxiliaryModels;
using AutoMapper;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping
{
    public class PersistenceToDomainProfile : Profile
    {
        public PersistenceToDomainProfile()
        {
            CreateMap<Entities.Distributor,IDistributor>().As<DistributorAuxiliaryModel>();
            CreateMap<Entities.Distributor,DistributorAuxiliaryModel>();
            
            CreateMap<Entities.Distributor,ScheduleDistributor.Domain.Models.Distributor>()
                .ConstructUsing(o => new ScheduleDistributor.Domain.Models.Distributor(o.Id,o.Name,o.DistributorType,o.ServerName,o.QueueName,o.CreatedAt));
        }
    }
}