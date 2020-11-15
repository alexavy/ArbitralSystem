namespace ArbitralSystem.Distributor.Core.Common
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