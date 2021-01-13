using System.Linq;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo.Models;

namespace ArbitralSystem.Repository.BulkRepository
{
    public class PairInfoRepository
    {
        private readonly DataBaseContext _ctx;
        
        public PairInfoRepository(DataBaseContext ctx)
        {
            Preconditions.CheckNotNull(ctx);
            _ctx = ctx;
        }

        public void Save(PairInfo pairInfo)
        {
           // _ctx.PairInfos.Add(pairInfo);
            _ctx.SaveChangesAsync();
        }
        
        public void Save(PairInfo[] pairInfos)
        {
            //_ctx.PairInfos.AddRangeAsync(pairInfos);
            _ctx.SaveChangesAsync();
        }
    }
}