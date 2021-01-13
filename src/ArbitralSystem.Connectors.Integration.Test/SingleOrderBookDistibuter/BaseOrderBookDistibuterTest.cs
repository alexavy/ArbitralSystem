using ArbitralSystem.Common.Logger;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Test;

namespace ArbitralSystem.Connectors.Integration.Test.SingleOrderBookDistributer
{
    public abstract class BaseOrderBookDistributerTest
    {
        private const int _timeBeforeCancelDistribution = 5 * 1000;
        private const int _timeBeforeCancelTask = 5 * 1000 * 2;

        protected BaseOrderBookDistributerTest()
        {
            DtoConverter = new CryptoExchangeConverter();
            Logger = new EmptyLogger();
        }

        protected IDtoConverter DtoConverter { get; }

        protected ILogger Logger { get; }

        protected int TimeBeforeCancelDistribution => _timeBeforeCancelDistribution;
        protected int TimeBeforeCancelTask => _timeBeforeCancelTask;
    }
}