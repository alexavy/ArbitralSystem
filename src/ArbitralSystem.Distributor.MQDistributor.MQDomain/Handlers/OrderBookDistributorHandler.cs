using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Handlers
{
    public class OrderBookDistributorCommandHandler : IRequestHandler<CreateOrderBookDistributorCommand, Guid>,
        IRequestHandler<UpdateOrderBookDistributorStatusCommand>, IRequestHandler<DeleteOrderBookDistributorByNameCommand> ,
        IRequestHandler<DeleteOrderBookDistributorCommand>
    {
        private readonly OrderBookDistributorDomainService _orderBookDistributorDomainService;
        private readonly IMediator _mediator;

        private readonly DistributorType _distributorType = DistributorType.OrderBooks;

        public OrderBookDistributorCommandHandler(OrderBookDistributorDomainService orderBookDistributorDomainService,
            IMediator mediator)
        {
            Preconditions.CheckNotNull(orderBookDistributorDomainService, mediator);
            _orderBookDistributorDomainService = orderBookDistributorDomainService;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateOrderBookDistributorCommand request, CancellationToken cancellationToken)
        {
            return await _orderBookDistributorDomainService.Create(request.ExchangePairInfo, cancellationToken);
        }

        public async Task<Unit> Handle(UpdateOrderBookDistributorStatusCommand request, CancellationToken cancellationToken)
        {
            await _orderBookDistributorDomainService.Update(request.DistributorId, request.Status, request.ServerId, cancellationToken);
            return Unit.Value;
        }
        
        public async Task<Unit> Handle(DeleteOrderBookDistributorByNameCommand request, CancellationToken cancellationToken)
        {
            var existedDistributor = await _mediator.Send(new ActiveDistributorByNameQuery(request.Name, _distributorType), cancellationToken);
            if (existedDistributor != null)
            {
                await _orderBookDistributorDomainService.Delete(existedDistributor.Id, cancellationToken);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteOrderBookDistributorCommand request, CancellationToken cancellationToken)
        {
            await _orderBookDistributorDomainService.Delete(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}