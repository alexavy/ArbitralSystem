using ArbitralSystem.Messaging.Options;
using MassTransit;

namespace ArbitralSystem.Messaging.Common
{
    public interface IBusControlFactory
    {
        IBusControl CreateInstance();
    }
}