using ArbitralSystem.Connectors.ArbitralPublicMarketInfoConnector;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Arbitral;
using ArbitralSystem.Distributor.MQDistributor.MQManagerService.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Extensions
{
    internal static class PublicMarketInfoExtension
    {
        public static void AddPublicMarketInfo(this IServiceCollection services, PublicMarketServiceConnectionInfo options)
        {
            services.AddSingleton<IConnectionInfo>(options);
            services.AddScoped<IPublicMarketInfoConnector, PublicMarketInfoConnector>();
        }
    }
}