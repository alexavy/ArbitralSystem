using ArbitralSystem.Connectors.Core;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common
{
    internal class PublicMarketServiceConnectionInfo : IConnectionInfo
    {
        public string BaseRestUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int DefaultTimeOutInMs { get; set;}
    }
}