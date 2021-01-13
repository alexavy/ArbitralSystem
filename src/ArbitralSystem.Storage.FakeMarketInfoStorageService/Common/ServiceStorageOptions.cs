using System;

namespace ArbitralSystem.Storage.FakeMarketInfoStorageService.Common
{
    internal class ServiceStorageOptions : ICloneable
    {
        public ushort PrefetchCount { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}