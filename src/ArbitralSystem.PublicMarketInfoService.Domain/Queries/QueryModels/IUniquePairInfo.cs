namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels
{
    public interface IUniquePairInfo 
    {
        string UnificatedPairName { get; }
        string BaseCurrency { get; }
        string QuoteCurrency { get; }
        int OccurrencesCount { get; }
    }
}