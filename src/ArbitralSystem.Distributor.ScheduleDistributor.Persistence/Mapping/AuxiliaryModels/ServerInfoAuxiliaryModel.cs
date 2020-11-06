using System;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping.AuxiliaryModels
{
    public class ServerInfoAuxiliaryModel : IServerInfo
    {
        public string Name { get; set; }
        public string Queue { get; set;}
        public int TotalCapacity { get; set;}
        public DateTimeOffset StartedAt { get; set;}
        public DateTimeOffset? HeartBeat { get; set;}
    }
}