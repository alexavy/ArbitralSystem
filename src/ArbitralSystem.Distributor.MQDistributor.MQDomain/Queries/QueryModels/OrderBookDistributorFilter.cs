using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Queries.QueryModels
{
    public class OrderBookDistributorFilter : DistributorFilter
    {
        public override DistributorType? Type => DistributorType.OrderBooks;
    }
}