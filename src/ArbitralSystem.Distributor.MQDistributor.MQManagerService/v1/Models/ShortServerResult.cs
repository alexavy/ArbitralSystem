namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Full server information, includes amount of distributor in not deleted status
    /// </summary>
    public class ShortServerResult : ServerResult
    {
        /// <summary>
        /// Amount of distributor in not deleted status
        /// </summary>
        public int ActiveWorkers { get; set; }
    }
}