using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;
using AutoMapper;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Mapping
{
    public class QueryToDomainMappingProfile : Profile
    {
        public QueryToDomainMappingProfile()
        {
            CreateMap<IPairInfo, PairInfo>().ConstructUsing((o, ctx) =>
                new PairInfo(o.Id, o.ExchangePairName, o.UnificatedPairName, o.BaseCurrency, o.QuoteCurrency, o.CreatedAt, o.DelistedAt, o.Exchange));
        }
    }
}