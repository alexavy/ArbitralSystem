using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries
{
    public class AvailablePairInfoFilterQuery : IRequest<IPairInfo>
    {
        public string UnificatedPairName { get; }
        public Exchange Exchange { get; }

        public AvailablePairInfoFilterQuery(string unificatedPairName, Exchange exchange)
        {
            UnificatedPairName = unificatedPairName;
            Exchange = exchange;
        }
    }
}