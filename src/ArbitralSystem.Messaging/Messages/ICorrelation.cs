using System;

namespace ArbitralSystem.Messaging.Messages
{
    public interface ICorrelation
    {
        Guid CorrelationId { get; }
    }
}
