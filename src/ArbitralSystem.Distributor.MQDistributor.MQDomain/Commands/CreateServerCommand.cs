using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Models;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands
{
    public class CreateServerCommand : IRequest<Guid>
    {
        public Server Server { get; }

        public CreateServerCommand(Server server)
        {
            Server = server;
        }
    }
}