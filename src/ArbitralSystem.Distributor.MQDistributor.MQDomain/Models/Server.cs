using System;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Common;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Models
{
    public class Server
    {
        public Server(Guid id, string name,ServerType type, int maxWorkers, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            ServerType = type;
            MaxWorkers = maxWorkers;
            CreatedAt = createdAt;
            ModifyAt = DateTimeOffset.Now;
        }
        
        public Server Update(Server server)
        {
            if(Id != server.Id)
                throw new InvalidOperationException("Can't update server, id is not equal.");
            if(IsDeleted)
                throw new InvalidOperationException("Can't update server, already set as deleted.");
            
            Name = server.Name;
            MaxWorkers = server.MaxWorkers;
            ModifyAt = DateTimeOffset.Now;
            
            return this;
        }

        public Server SetAsDeleted()
        {
            IsDeleted = true;
            ModifyAt = DateTimeOffset.Now;

            return this;
        }

        public Guid Id { get; }
        public string Name { get; private set;}
        public ServerType ServerType { get; }
        public int MaxWorkers { get; private set; }
        public DateTimeOffset CreatedAt { get; }
        public DateTimeOffset? ModifyAt { get; private set; }
        public bool IsDeleted { get; private set;}
        
        //only for mapping from persistence layer
        public Server(Guid id, string name, ServerType type , int maxWorkers,  DateTimeOffset createdAt, DateTimeOffset? modifyAt, bool isDeleted)
        {
            Id = id;
            Name = name;
            ServerType = type;
            MaxWorkers = maxWorkers;
            CreatedAt = createdAt;
            ModifyAt = modifyAt;
            IsDeleted = isDeleted;
        }
    }
}