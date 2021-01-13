using System;
using System.Collections.Generic;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using JetBrains.Annotations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Mapping.AuxiliaryModels
{
    [UsedImplicitly]
    internal class ServerAuxiliaryModel : IServer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServerType ServerType { get; set; }
        public int MaxWorkersCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifyAt { get; set; }
        public ICollection<IDistributor> Distributors { get; set; }
        public bool IsDeleted { get; set; }
    }
}