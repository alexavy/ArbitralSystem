using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Jobs;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Distributer.OrderBookDistributorCore.Messaging.Extensions
{
    public static class OrderBookDistributorExtension
    {
        public static void AddOrderBookDistributor(this IServiceCollection services, DistributionOptions options)
        {
            services.AddSingleton(options);
            
            services.AddTransient<DistributorJob>();
            services.AddTransient<IOrderBookDistributerFactory, CryptoExOrderBookDistributerFactory>();
        }
    }
}