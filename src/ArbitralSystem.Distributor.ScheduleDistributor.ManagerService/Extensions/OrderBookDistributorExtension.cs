using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Jobs;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Models;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Stubs;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Extensions
{
    internal static class OrderBookDistributorExtension
    {
        public static void AddOrderBookDistributorStub(this IServiceCollection services)
        {
            services.AddSingleton(new DistributionOptions());
            services.AddSingleton<IDtoConverter, DtoConverterStub>();
            services.AddTransient<IOrderBookPublisher, OrderBookPublisherStub>();
            services.AddTransient<IOrderBookDistributerFactory, OrderBookDistributerFactoryStub>();
            
            services.AddTransient<OrderBookDistributorJob>();
        }
    }
}