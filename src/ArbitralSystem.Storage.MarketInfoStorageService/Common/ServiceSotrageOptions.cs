

using JetBrains.Annotations;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Common
{
    [UsedImplicitly]
    public class ServiceStorageOptions : IServiceStorageOptions
    {
        public ushort PrefetchCount { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
