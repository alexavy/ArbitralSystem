namespace ArbitralSystem.Distributor.MarketInfoAggregatorService.Services
{
    public interface ITimeLimitedAggregator<T> : IAggregator<T> where T : class
    {
        void StartTimer();

        void StopTimer();
    }
}