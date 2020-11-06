using JetBrains.Annotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Common
{
    [UsedImplicitly]
    internal class DistributorManagerSettings
    {
        public string DatabaseConnection { get; set; }
        public string MqHost { get; set; }
    }
}