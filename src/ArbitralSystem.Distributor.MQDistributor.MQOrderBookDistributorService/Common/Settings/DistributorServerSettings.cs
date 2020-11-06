using System;
using JetBrains.Annotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Common.Settings
{
    [UsedImplicitly]
    public class DistributorServerSettings : ICloneable
    {
        /// <summary>
        /// Server name must be unique.
        /// </summary>
        public string ServerName { get; set; }
        
        /// <summary>
        /// Max jobs which can be executed at the same time. 
        /// </summary>
        public ushort MaxWorkersCount { get; set; }

        public ushort HeartBeatIntervalInSeconds { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    
    
}