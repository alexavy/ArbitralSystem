using System;
using ArbitralSystem.Messaging.Models;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IServerCreatedMessage : ICorrelation
    {
        public Guid ServerId { get; }
        public string Name { get; }
        public ServerType ServerType { get; }
        public int MaxWorkersCount { get; }
        public DateTimeOffset CreatedAt { get; }
    }
    
    public interface IServerDeletedMessage : ICorrelation
    {
        public Guid ServerId { get; }
    }
}