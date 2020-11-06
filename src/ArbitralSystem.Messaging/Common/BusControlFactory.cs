using ArbitralSystem.Messaging.Options;
using MassTransit;

namespace ArbitralSystem.Messaging.Common
{
    public class BusControlFactory : IBusControlFactory
    {
        private readonly IConnectionOptions _options;

        public BusControlFactory(IConnectionOptions options)
        {
            _options = options;
        }
        
        public IBusControl CreateInstance()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg => { cfg.Host(_options.Host); });
        }
    }
}