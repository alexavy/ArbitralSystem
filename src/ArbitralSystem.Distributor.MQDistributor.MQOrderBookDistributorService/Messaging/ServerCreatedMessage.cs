using System;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Messaging
{
    internal class ServerCreatedMessage : BaseMessage, IServerCreatedMessage
    {
        public ServerCreatedMessage(Guid serverId, string name,ServerType serverType , int maxWorkersCount, DateTimeOffset createdAt)
        {
            ServerId = serverId;
            Name = name;
            ServerType = serverType;
            MaxWorkersCount = maxWorkersCount;
            CreatedAt = createdAt;
        }

        public Guid ServerId { get; }
        public string Name { get; }
        public ServerType ServerType { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
    }
}