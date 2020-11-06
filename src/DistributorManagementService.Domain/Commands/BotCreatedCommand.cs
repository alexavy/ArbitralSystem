using System;
using DistributorManagementService.Domain.Models;
using MediatR;

namespace DistributorManagementService.Domain.Commands
{
    public class BotCreatedCommand : IRequest<IDistributor>
    {
        public Guid Id { get; }
        public string Name { get; }

        public BotCreatedCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    
    public class BotDeleteCommand : IRequest<IDistributor>
    {
        public Guid Id { get; }

        public BotDeleteCommand(Guid id)
        {
            Id = id;
        }
    }
}