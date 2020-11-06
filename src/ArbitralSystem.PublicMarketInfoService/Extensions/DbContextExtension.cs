using System;
using ArbitralSystem.PublicMarketInfoService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArbitralSystem.PublicMarketInfoService.Extensions
{
    internal static class DbContextExtension
    {
        public static void AddArbitralSystemDbContext(this IServiceCollection services, IConfigurationRoot configuration)
        {
            Func<PublicMarketInfoBdContext> dbContextCreationFunc = () =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<PublicMarketInfoBdContext>()
                    .UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"])
                    .Options;

                return new PublicMarketInfoBdContext(dbContextOptions);
            };
            services.AddSingleton(dbContextCreationFunc);
            services.AddDbContext<PublicMarketInfoBdContext>(
                (provider, builder) => builder.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"])
            );
        }
    }
}