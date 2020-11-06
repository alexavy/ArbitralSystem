using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Commands;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Handlers
{
    public class DeleteOrderBookDistributorCommandHandler : IRequestHandler<DeleteOrderBookDistributorCommand>,
        IRequestHandler<DeleteAllOrderBookDistributorsCommand>
    {
        private readonly OrderBookDistributorDomainService _orderBookDistributorDomainService;
        private readonly IMediator _mediator;
        
        public DeleteOrderBookDistributorCommandHandler(OrderBookDistributorDomainService orderBookDistributorDomainService,
            IMediator mediator)
        {
            _orderBookDistributorDomainService = orderBookDistributorDomainService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteOrderBookDistributorCommand request, CancellationToken cancellationToken)
        {
            await _orderBookDistributorDomainService.DeleteDistributor(request.Name, cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteAllOrderBookDistributorsCommand request, CancellationToken cancellationToken)
        {
            var existedDistributors = await _mediator.Send(new DistributorQuery(type: request.DistributorType),cancellationToken);
            foreach (var distributor in existedDistributors)
            {
                await _orderBookDistributorDomainService.DeleteDistributor(distributor.Name, cancellationToken);
            }
            return Unit.Value;
        }
    }
}