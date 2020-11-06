using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Commands;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Handlers
{
    public class CreateOrderBookDistributorCommandHandler : IRequestHandler<CreateOrderBookDistributorCommand,string>
    {
        private readonly OrderBookDistributorDomainService _orderBookDistributorDomainService;
        public CreateOrderBookDistributorCommandHandler(OrderBookDistributorDomainService orderBookDistributorDomainService)
        {
            _orderBookDistributorDomainService = orderBookDistributorDomainService;
        }
        
        public async Task<string> Handle(CreateOrderBookDistributorCommand request, CancellationToken cancellationToken)
        {
            return await _orderBookDistributorDomainService.CreateDistributor(request.ExchangePairInfo, cancellationToken);
        }
    }
}