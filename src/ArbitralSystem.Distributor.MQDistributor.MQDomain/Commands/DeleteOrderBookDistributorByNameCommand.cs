using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands
{
    public class DeleteOrderBookDistributorByNameCommand : IRequest
    {
        public string Name { get; }

        public DeleteOrderBookDistributorByNameCommand(string name)
        {
            Name = name;
        }
    }
}