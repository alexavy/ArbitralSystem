using System;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Messaging.Options;
using DistributorManagementService.Consumers;
using GreenPipes;
using MassTransit;
using MassTransit.Context;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributorManagementService.Extensions
{
    public static class ArbitralMassTransitExtension
    {
        public static void AddArbitralBus(this IServiceCollection  services, IConnectionOptions mqOptions)
        {
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<DistributorStateConsumer>();

                MessageCorrelation.UseCorrelationId<IDistributorMessage>(o => o.CorrelationId);

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(mqOptions.Host), h => { });
                    
                    cfg.ReceiveEndpoint(Constants.Queues.ControlServiceDistributorStates, e =>
                    {
                        e.PrefetchCount = 1;
                        e.ConfigureConsumer<DistributorStateConsumer>(provider);
                    });
                }));
            });
        }
    }
}