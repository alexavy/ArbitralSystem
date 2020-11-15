namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Options
{
    public class DistributorServiceOptions
    {
        public string ServerId { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string MqConnectionString { get; set; }
        public int MaxWorkersCount { get; set; }
        public int? Trim { get; set; }
    }
}