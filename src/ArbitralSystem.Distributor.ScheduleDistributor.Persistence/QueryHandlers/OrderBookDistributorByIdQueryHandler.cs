using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Pagination.SqlServer;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Mapping.AuxiliaryModels;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence.QueryHandlers.Specs;
using AutoMapper;
using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.QueryHandlers
{
    public class OrderBookDistributorByIdQueryHandler : Query, IRequestHandler<OrderBookDistributorByIdQuery, IOrderBookDistributor>,
        IRequestHandler<OrderBookDistributorQuery, Page<IOrderBookDistributor>>
    {
        private readonly IMonitoringApi _monitoringApi;

        public OrderBookDistributorByIdQueryHandler(DistributorDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _monitoringApi = JobStorage.Current.GetMonitoringApi();
        }

        public async Task<IOrderBookDistributor> Handle(OrderBookDistributorByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .AsNoTracking().FirstOrDefaultAsync(o => o.Id.Equals(request.Id), cancellationToken);

            if (result != null)
            {
                var job = _monitoringApi.JobDetails(result.Id);
                var exchangePairs = job.Job.Args.First() as DistributorExchangePairs;
                return new OrderBookDistributorAuxiliaryModel(Mapper.Map<IDistributor>(result), exchangePairs?.ExchangePairInfo.UnificatedPairName,
                    exchangePairs?.ExchangePairInfo.PairInfos.Select(o => o.Exchange));
            }

            return null;
        }

        public async Task<Page<IOrderBookDistributor>> Handle(OrderBookDistributorQuery request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Distributors
                .AsNoTracking()
                .Where(o => o.DistributorType.ToLower().Equals(request.DistributorType.ToLower()))
                .Where(DistributorSpecs.ByServerName(request.Filter.ServerName))
                .Where(DistributorSpecs.ByQueueName(request.Filter.QueueName))
                .Where(DistributorSpecs.ByName(request.Filter.Name))
                .OrderBy(o => o.CreatedAt)
                .Page(request.Filter, cancellationToken);
            
            var orderBookDistributors = new List<IOrderBookDistributor>();
            foreach (var distributor in result.Items)
            {
                var job = _monitoringApi.JobDetails(distributor.Id);
                var exchangePairs = job.Job.Args.First() as DistributorExchangePairs;
                var orderBookDistributor = new OrderBookDistributorAuxiliaryModel(Mapper.Map<IDistributor>(distributor),
                    exchangePairs?.ExchangePairInfo.UnificatedPairName,
                    exchangePairs?.ExchangePairInfo.PairInfos
                        .Select(o => o.Exchange));
                orderBookDistributors.Add(orderBookDistributor);
            }

            return new Page<IOrderBookDistributor>(orderBookDistributors, result.Total);
        }
    }
}