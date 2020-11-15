using System;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Common
{
    public interface IServiceStorageOptions : ICloneable
    {
        ushort PrefetchCount { get; }
    }
}