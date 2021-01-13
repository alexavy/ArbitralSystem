using System;
using ArbitralSystem.Messaging.Messages;

namespace ArbitralSystem.Distributor.MQDistributor.MQOrderBookDistributorService.Messaging
{
    internal class ServerDeletedMessage : BaseMessage, IServerDeletedMessage
    {
        public ServerDeletedMessage(Guid serverId)
        {
            ServerId = serverId;
        }

        public Guid ServerId { get;}
    }
}