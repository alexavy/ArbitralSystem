namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Common
{
    public abstract class BaseOrderBookDistributorRequest
    {
        public string DistributorType { get; } = DistributorConstants.OrderBookDistributorIdentity;
    }

    public static class DistributorConstants
    {
        public static string OrderBookDistributorIdentity = "orderbook";
    }
}