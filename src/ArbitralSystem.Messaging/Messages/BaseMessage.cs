using System;

namespace ArbitralSystem.Messaging.Messages
{
    public abstract class BaseMessage : ICorrelation
    {
        protected BaseMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; }
    }
}
