using System;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.v1.Models
{
    /// <summary>
    /// Server full information
    /// </summary>
    public class ServerResult
    {
        /// <summary>
        /// Server id.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Server working type.
        /// </summary>
        public ServerType ServerType { get; set; }
        
        /// <summary>
        /// Max count of possible workers.
        /// </summary>
        public int MaxWorkersCount { get; set; }
        
        /// <summary>
        /// Created at.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        
        /// <summary>
        /// Updated at.
        /// </summary>
        public DateTimeOffset? ModifyAt { get; set; }
        
        /// <summary>
        /// Is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}