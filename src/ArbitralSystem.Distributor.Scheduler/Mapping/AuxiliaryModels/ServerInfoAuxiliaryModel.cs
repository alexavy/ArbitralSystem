using System;
using ArbitralSystem.Distributor.Domain.Queries.QueryModels;

namespace ArbitralSystem.Distributor.Scheduler.Mapping.AuxiliaryModels
{
    public class ServerInfoAuxiliaryModel : IServerInfo
    {
        public string Name { get; set; }
        public string Queue { get; set; }
        public int TotalCapacity { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? HeartBeat { get; set; }
    }
}