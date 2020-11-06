using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Commands
{
    public class DeleteOrderBookDistributorCommand : BaseOrderBookDistributorRequest, IRequest
    {
        public string Name { get; }

        public DeleteOrderBookDistributorCommand(string name)
        {
            Name = name;
        }
    }
}