namespace ArbitralSystem.Distributor.ScheduleDistributor.OrderBookDistributorService.Options
{
    internal class ServerOptions
    {
        public string ServerName { get; }
        public string ServerQueueName { get; }

        public ServerOptions(string serverName, string serverQueueName)
        {
            ServerName = serverName;
            ServerQueueName = serverQueueName;
        }
    }
}