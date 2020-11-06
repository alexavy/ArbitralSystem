using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public interface IServer
    {
        public Guid Id { get; }
        public string Name { get; }
        public ServerType ServerType { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; }
        public ICollection<IDistributor> Distributors { get; }
        public bool IsDeleted { get; }
    }
}