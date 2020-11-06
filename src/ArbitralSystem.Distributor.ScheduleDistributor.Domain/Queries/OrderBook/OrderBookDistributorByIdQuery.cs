using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Queries.OrderBook
{
    public class OrderBookDistributorByIdQuery : BaseOrderBookDistributorRequest, IRequest<IOrderBookDistributor>
    {
        public string Id { get; }

        public OrderBookDistributorByIdQuery(string id)
        {
            Id = id;
        }
    }
}