using System;
using ArbitralSystem.Distributor.MQDistributor.MQPersistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Distributor.MQDistributor.MQManagerService.Extensions
{
    internal static class ArbitralSystemDbContext
    {
        public static void AddArbitralSystemDbContext(this IServiceCollection services, string connectionString)
        {
            Func<DistributorDbContext> dbContextCreationFunc = () =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<DistributorDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                return new DistributorDbContext(dbContextOptions);
            };
            services.AddSingleton(dbContextCreationFunc);
            services.AddDbContext<DistributorDbContext>(
                (provider, builder) => builder.UseSqlServer(connectionString)
            );
        }
    }
}