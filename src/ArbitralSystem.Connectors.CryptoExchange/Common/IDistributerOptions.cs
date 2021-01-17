using System;
using ArbitralSystem.Domain.MarketInfo;

namespace ArbitralSystem.Connectors.CryptoExchange.Common
{
    public interface IDistributerOptions : ICloneable
    {
        Exchange Exchange { get; }

        int? Frequency { get; }

        int? Limit { get; }
        
        int SilenceLimitInSeconds { get; } 
    }
}