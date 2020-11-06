using System.Linq;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.Filters;
using ArbitralSystem.PublicMarketInfoService.Persistence.Entities;
using ArbitralSystem.PublicMarketInfoService.Persistence.Queries.Specs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Queries
{
    public abstract class BasePairInfoFilterQueryHandler : Query
    {
        protected BasePairInfoFilterQueryHandler(PublicMarketInfoBdContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public virtual IQueryable<PairInfo> FullPairInfoFilterQuery(PairInfoFilter filter)
        {
            return DbContext.PairInfos.AsNoTracking()
                .Where(PairInfoSpec.ByExchange(filter.Exchange))
                .Where(PairInfoSpec.ByUnificatedPairName(filter.UnificatedPairName))
                .Where(PairInfoSpec.ByExchangePairName(filter.ExchangePairName))
                .Where(PairInfoSpec.ByBaseCurrency(filter.BaseCurrency))
                .Where(PairInfoSpec.ByQuoteCurrency(filter.QuoteCurrency))
                .Where(PairInfoSpec.ByIsDelisted(filter.IsDelisted))
                .Where(PairInfoSpec.ByListedMoreThan(filter.ListedMoreThan, filter.Exchange, DbContext));
        }
    }
}