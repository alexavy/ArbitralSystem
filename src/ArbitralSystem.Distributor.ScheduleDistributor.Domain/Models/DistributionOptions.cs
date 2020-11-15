namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models
{
    public class DistributionOptions
    {
        public int? TrimOrderBookDepth { get; }
        public DistributionOptions(int? trimOrderBookDepth = null)
        {
            TrimOrderBookDepth = trimOrderBookDepth;
        }
    }
}