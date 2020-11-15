using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using AutoMapper;
using Hangfire;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.QueryHandlers
{
    public class ServerInfoQueryHandler : IRequestHandler<OrderBookDistributorServerInfoQuery,IEnumerable<IServerInfo>>
    {
        private readonly IMapper _mapper;

        public ServerInfoQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public Task<IEnumerable<IServerInfo>> Handle(OrderBookDistributorServerInfoQuery request, CancellationToken cancellationToken)
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var rawServers = monitoringApi.Servers()
                .Where(o => o.Name.StartsWith(request.DistributorType));

            return Task.FromResult(_mapper.Map<IEnumerable<IServerInfo>>(rawServers));
        }
    }
}