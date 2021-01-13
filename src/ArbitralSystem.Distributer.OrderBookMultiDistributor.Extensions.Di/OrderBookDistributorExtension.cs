using System;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributor.Extensions.Di
{
    public static  class OrderBookDistributorExtension
    {
        public static void AddOrderBookDistributor(this IServiceCollection services)
        {
            services.AddScoped<IOrderBookDistributerFactory,CryptoExOrderBookDistributerFactory>();
        }
    }
}