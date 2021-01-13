using System.Linq;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries.QueryModels;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Mapping.AuxiliaryModels
{
    internal class UniquePairInfoPreparer : IUniquePairInfo
    {
        public UniquePairInfoPreparer(string unificatedPairName, int occurrencesCount) 
        {
            UnificatedPairName = unificatedPairName;
            OccurrencesCount = occurrencesCount;
            var parts = unificatedPairName.Split('/');
            if (parts.Any())
            {
                if(parts.Length > 1)
                    BaseCurrency = parts[0];
                if(parts.Length >= 2)
                    QuoteCurrency = parts[1];
            }
        }

        public string UnificatedPairName { get; }
        public string BaseCurrency { get; }
        public string QuoteCurrency { get; }
        public int OccurrencesCount { get; }
    }
}