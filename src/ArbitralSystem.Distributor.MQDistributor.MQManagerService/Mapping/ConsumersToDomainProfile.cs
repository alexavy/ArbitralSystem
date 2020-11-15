using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Models;
using ArbitralSystem.Messaging.Messages;
using AutoMapper;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Mapping
{
    internal class ConsumersToDomainProfile : Profile 
    {
        public ConsumersToDomainProfile()
        {
            CreateMap<Messaging.Models.ServerType, ServerType>();
            CreateMap<Messaging.Models.DistributorStatus, DistributorStatus>();
            CreateMap<Messaging.Models.HeartBeatOrderBookDistributor, DistributorHeartBeat>()
                .ConstructUsing( (o,ctx)=> 
                    new DistributorHeartBeat(o.DistributorId,
                        o.Exchange,
                        o.DateTimeOffset));
            
            CreateMap<IServerCreatedMessage, Server>()
                .ConstructUsing( (o,ctx)=> 
                    new Server(o.ServerId,
                        o.Name,
                        ctx.Mapper.Map<ServerType>(o.ServerType),
                        o.MaxWorkersCount,
                        o.CreatedAt));
        }
    }
}