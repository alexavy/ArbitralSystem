using System;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels
{
    public interface IServerInfo
    {
        string Name { get; }
        string Queue { get; }
        int TotalCapacity { get; }
        DateTimeOffset StartedAt { get; }
        DateTimeOffset? HeartBeat { get; }
    }
}