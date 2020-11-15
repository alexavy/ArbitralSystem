using System;
using ArbitralSystem.Storage.MarketInfoStorageService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Extensions
{
    public static class DbContextExtension
    {
        public static void AddArbitralSystemDbContext(this IServiceCollection services, string databaseConnection)
        {
            Func<MarketInfoBdContext> dbContextCreationFunc = () =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<MarketInfoBdContext>()
                    .UseSqlServer(databaseConnection)
                    .Options;

                return new MarketInfoBdContext(dbContextOptions);
            };
            services.AddSingleton(dbContextCreationFunc);
            services.AddDbContext<MarketInfoBdContext>(
                (provider, builder) => builder.UseSqlServer(databaseConnection)
            );
        }
    }
}