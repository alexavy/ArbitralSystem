using System.Linq;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping.AuxiliaryModels;
using AutoMapper;
using Hangfire.Storage.Monitoring;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping
{
    public class SchedulerToDomainProfile : Profile
    {
        public SchedulerToDomainProfile()
        {
            CreateMap<ServerDto, IServerInfo>().As<ServerInfoAuxiliaryModel>();
            CreateMap<ServerDto, ServerInfoAuxiliaryModel>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Queue, o => o.MapFrom(src => src.Queues.First()))
                .ForMember(dest => dest.TotalCapacity, o => o.MapFrom(src => src.WorkersCount))
                .ForMember(dest => dest.StartedAt, o => o.MapFrom(src => src.StartedAt))
                .ForMember(dest => dest.HeartBeat, o => o.MapFrom(src => src.Heartbeat));
        }
        
    }
}