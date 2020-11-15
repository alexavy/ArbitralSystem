using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook
{
    public class OrderBookDistributorQuery : BaseOrderBookDistributorRequest, IRequest<Page<IOrderBookDistributor>>
    {
        public DistributorFilter Filter { get; }

        public OrderBookDistributorQuery(DistributorFilter filter)
        {
            Filter = filter;
        }
    }
}