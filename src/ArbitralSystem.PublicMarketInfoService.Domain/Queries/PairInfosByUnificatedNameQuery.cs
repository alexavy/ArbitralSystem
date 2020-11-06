using System.Collections.Generic;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries
{
    public class PairInfosByUnificatedNameQuery : IRequest<IEnumerable<IPairInfo>>
    {
        public string UnificatedPairName { get; }
        public PairInfosByUnificatedNameQuery(string unificatedPairName)
        {
            UnificatedPairName = unificatedPairName;
        }
    }
}