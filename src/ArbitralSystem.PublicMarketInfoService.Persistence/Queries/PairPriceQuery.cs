using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Pagination;
using ArbitralSystem.Common.Pagination.SqlServer;
using ArbitralSystem.Domain.MarketInfo;
using ArbitralSystem.PublicMarketInfoService.Domain.Common.Exceptions;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using ArbitralSystem.PublicMarketInfoService.Persistence.Entities;
using ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Auxiliary;
using ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Specs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries
{
    public class PairPriceQueryHandler : Query, IRequestHandler<PairPriceQuery, Page<IPairPrice>>,
        IRequestHandler<SummaryPairPriceQuery,IPairPriceSummaryInfo>
    {
        public PairPriceQueryHandler(PublicMarketInfoBdContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }


        public async Task<Page<IPairPrice>> Handle(PairPriceQuery request, CancellationToken cancellationToken)
        {
            var pairInfo = await GetPairInfoAsync(request.UnificatedPairName, request.Exchange, cancellationToken);

            return await DbContext.PairPrices.AsNoTracking()
                .Where(o => o.Exchange == request.Exchange)
                .Where(o => o.ExchangePairName == pairInfo.ExchangePairName)
                .Where(PairPriceSpec.ByFrom(request.PageFilter.From))
                .Where(PairPriceSpec.ByTo(request.PageFilter.To))
                .Select(o => new PairPriceAuxiliaryModel()
                    {
                        Exchange = o.Exchange,
                        ExchangePairName = o.ExchangePairName,
                        Date = o.Date,
                        Price = o.Price,
                        UnificatedPairName = pairInfo.UnificatedPairName
                    }
                )
                .OrderBy(o => o.Date)
                .Page(request.PageFilter, Mapper.Map<IPairPrice>, cancellationToken);
        }

        public async Task<IPairPriceSummaryInfo> Handle(SummaryPairPriceQuery request, CancellationToken cancellationToken)
        {
            var pairInfo = await GetPairInfoAsync(request.UnificatedPairName, request.Exchange, cancellationToken);

            var baseInformation = await DbContext.PairPrices.AsNoTracking()
                .Where(o => o.Exchange == request.Exchange)
                .Where(o => o.ExchangePairName == pairInfo.ExchangePairName)
                .Where(PairPriceSpec.ByFrom(request.PageFilter.From))
                .Where(PairPriceSpec.ByTo(request.PageFilter.To))
                .GroupBy(o => o.ExchangePairName)
                .Select(o =>
                    new
                    {
                        Max = o.Max(x => x.Price),
                        Min = o.Min(x => x.Price),
                        Avereage = o.Average(x => x.Price)
                    })
                .FirstOrDefaultAsync(cancellationToken);

            if (baseInformation == null)
                throw new NoDataForPeriodException(request.UnificatedPairName, request.Exchange);
            
            return new PairPriceSummaryInfoAuxiliaryModel()
            {
                MaxPrice = baseInformation?.Max,
                MinPrice = baseInformation?.Min,
                AveragePrice = baseInformation?.Avereage,
                Exchange = pairInfo.Exchange,
                ExchangePairName = pairInfo.ExchangePairName,
                UnificatedPairName = pairInfo.UnificatedPairName
            };
        }

        private async Task<PairInfo> GetPairInfoAsync(string unificatedPairName, Exchange exchange, CancellationToken token)
        {
            var pairInfo = await DbContext.PairInfos.FirstOrDefaultAsync(o => o.UnificatedPairName == unificatedPairName &&
                                                                              o.Exchange == exchange &&
                                                                              o.DelistedAt == null, token);
            if (pairInfo == null)
                throw new NoSuchPairException(unificatedPairName, exchange);
            return pairInfo;
        }
    }
}