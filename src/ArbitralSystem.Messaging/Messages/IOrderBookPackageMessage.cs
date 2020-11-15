using System.Collections.Generic;

namespace ArbitralSystem.Messaging.Messages
{
    public interface IOrderBookPackageMessage : ICorrelation
    {
        IEnumerable<IOrderBookMessage> OrderBooks { get; }
    }
}