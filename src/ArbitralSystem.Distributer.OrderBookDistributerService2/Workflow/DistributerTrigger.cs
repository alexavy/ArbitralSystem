namespace ArbitralSystem.Distributer.OrderBookDistributerService.Workflow
{
    internal enum DistributerTrigger
    { 
        StartListenForPair,
        StartDistribution,
        StopDistribution,
        Dispose,
    }
}