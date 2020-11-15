using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public interface IServerReference
    {
        public Guid Id { get; }
        public string Name { get; }
        public ServerType ServerType { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; }
        public bool IsDeleted { get; }
    }
}