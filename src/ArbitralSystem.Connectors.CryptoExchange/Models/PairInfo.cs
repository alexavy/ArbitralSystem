using System.Runtime.CompilerServices;
using ArbitralSystem.Connectors.Core.Models;
using ArbitralSystem.Domain.MarketInfo;

[assembly:InternalsVisibleTo("ArbitralSystem.Connectors.Test")]
namespace ArbitralSystem.Connectors.CryptoExchange.Models
{
    internal class PairInfo : IPairInfo
    {
        public Exchange Exchange { get ; set; }
        public string UnificatedPairName => GetUnificatedPairName(BaseCurrency, QuoteCurrency);
        public string ExchangePairName { get; set; }
        public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set; }
        
        private string GetUnificatedPairName(string baseCurrency, string quoteCurrency)
        {
            baseCurrency = baseCurrency?.ToUpper();
            quoteCurrency = quoteCurrency?.ToUpper();
            if (baseCurrency != null && quoteCurrency != null)
                return $"{baseCurrency}/{quoteCurrency}";
            return string.Empty;
        }
    }
}