using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core.Arbitral;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorService.Extensions
{
    public static  class OrderBookDistributorExtension
    {
        public static void AddOrderBookDistributor(this IServiceCollection services)
        {
            services.AddScoped<IOrderBookDistributerFactory,CryptoExOrderBookDistributerFactory>();
            services.AddSingleton<IDtoConverter, CryptoExchangeConverter>();
            //services.AddTransient<ICoinExConnector, CoinExConnector>();
            
            
        }
    }
}