using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.Server
{
    public class ServerQueryById : IRequest<IServer>
    {
        public Guid ServerId { get; }

        public bool? IsDeletedDistributors { get; }

        public ServerQueryById(Guid serverId, bool? isDeletedDistributors = null)
        {
            ServerId = serverId;
            IsDeletedDistributors = isDeletedDistributors;
        }
    }
}