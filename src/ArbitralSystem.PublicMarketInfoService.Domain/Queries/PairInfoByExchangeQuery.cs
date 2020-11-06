using System.Collections.Generic;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Queries
{
    public class PairInfoByExchangeQuery : IRequest<IEnumerable<IPairInfo>>
    {
        public bool? IsActive { get; }
        public Exchange Exchange { get; }
        public PairInfoByExchangeQuery(Exchange exchange, bool? isActive = null)
        {
            IsActive = isActive;
            Exchange = exchange;
        }
    }
}