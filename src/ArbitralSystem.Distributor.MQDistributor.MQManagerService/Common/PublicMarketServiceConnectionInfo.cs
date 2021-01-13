using ArbitralSystem.Connectors.Core;
using JetBrains.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Common
{
    [UsedImplicitly]
    internal class PublicMarketServiceConnectionInfo : IConnectionInfo
    {
        public string BaseRestUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int DefaultTimeOutInMs { get; set;}
    }
}