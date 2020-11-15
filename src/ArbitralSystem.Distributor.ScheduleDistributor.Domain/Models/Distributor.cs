using System;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models
{
    public class Distributor
    {
        public string Id { get; }
        public string Name { get; }
        public string DistributorType { get; }
        public string ServerName { get; }
        public string QueueName { get; }
        public DateTimeOffset CreatedAt { get; }
        
        public Distributor(string id, string name, string distributorType, string serverName, string queueName, DateTimeOffset createdAt)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(distributorType) || string.IsNullOrEmpty(serverName) || 
               string.IsNullOrEmpty(queueName))
                throw new ArgumentException("Distributor args is not valid.");
            
            Id = id;
            Name = name;
            DistributorType = distributorType;
            ServerName = serverName;
            QueueName = queueName;
            CreatedAt = createdAt;
        }
    }
}