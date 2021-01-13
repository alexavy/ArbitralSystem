using System;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands
{
    public class DeleteServerCommand : IRequest
    {
        public Guid ServerId { get; }

        public DeleteServerCommand(Guid serverId)
        {
            ServerId = serverId;
        }
    }
}