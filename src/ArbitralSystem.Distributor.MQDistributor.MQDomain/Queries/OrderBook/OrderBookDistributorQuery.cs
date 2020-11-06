using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.OrderBook
{
    public class OrderBookDistributorQuery :  IRequest<Page<IOrderBookDistributor>>
    {
        public OrderBookDistributorFilter Filter { get; }

        public OrderBookDistributorQuery(OrderBookDistributorFilter filter)
        {
            Filter = filter;
        }
    }
}