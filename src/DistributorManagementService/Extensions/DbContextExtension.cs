using System;
using DistributorManagementService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributorManagementService.Extensions
{
    public static class DbContextExtension
    {
        public static void AddArbitralSystemDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            Func<DistributorDbContext> dbContextCreationFunc = () =>
            {
                var dbContextOptions = new DbContextOptionsBuilder<DistributorDbContext>()
                    .UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"])
                    .Options;

                return new DistributorDbContext(dbContextOptions);
            };
            services.AddSingleton(dbContextCreationFunc);
            services.AddDbContext<DistributorDbContext>(
                (provider, builder) => builder.UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"])
            );
        }
    }
}