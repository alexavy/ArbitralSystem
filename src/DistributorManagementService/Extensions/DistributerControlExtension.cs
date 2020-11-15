using System;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributorManagementService.Extensions
{
    public static class DistributerControlExtension
    {
        private static string DomainAssembly => "DistributorManagementService.Domain";
        private static string PersistenceAssembly => "DistributorManagementService.Persistence";
        
        public static void AddDistributerControlServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(AppDomain.CurrentDomain.Load(DomainAssembly));
            services.AddMediatR(AppDomain.CurrentDomain.Load(PersistenceAssembly));
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                AppDomain.CurrentDomain.Load(PersistenceAssembly));
        }
    }
}