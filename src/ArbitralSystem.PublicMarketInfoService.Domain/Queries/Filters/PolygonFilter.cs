namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters
{
    public class PolygonFilter
    {
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        public string UnificatedPairName { get; set; }
    }
}