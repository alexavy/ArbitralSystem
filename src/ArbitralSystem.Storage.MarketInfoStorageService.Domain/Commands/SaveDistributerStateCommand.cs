using System;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;
using MediatR;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Domain.Commands
{
    public class SaveDistributerStateCommand : IRequest<Guid>
    {
        public DistributerState State { get; }

        public SaveDistributerStateCommand(DistributerState state)
        {
            State = state;
        }
    }
}