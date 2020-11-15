using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Handlers
{
    public class HeartBeatHandler : IRequestHandler<OrderBookDistributorHeartBeatCommand>
    {
        private readonly IHeartBeatRepository _heartBeatRepository;

        public HeartBeatHandler(IHeartBeatRepository heartBeatRepository)
        {
            _heartBeatRepository = heartBeatRepository;
        }
        
        public async Task<Unit> Handle(OrderBookDistributorHeartBeatCommand request, CancellationToken cancellationToken)
        {
            if (request.HeartBeats.Any())
                await _heartBeatRepository.Update(request.HeartBeats.ToArray(), cancellationToken);
            
            return Unit.Value;
        }
    }
}