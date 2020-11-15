using System;
using System.Reflection;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain;
using ArbitralSystem.Distributor.ScheduleDistributor.Domain.Interfaces;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Extensions;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Common.Auth;
using ArbitralSystem.Distributor.ScheduleDistributor.ManagerService.Services;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence;
using ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Repositories;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace ArbitralSystem.Distributor.ScheduleDistributor.ManagerService
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;
        
        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var marketInfoConnectionInfo = _configuration.GetSection("Connectors:PublicMarketService")
                .Get<PublicMarketServiceConnectionInfo>();
            
            var distributorManagerOptions = _configuration.GetSection("DistributorSettings")
                .Get<DistributorManagerSettings>();
            
            services.AddMvcCore(config =>
                {
                    config.EnableEndpointRouting = false;
                    config.RespectBrowserAcceptHeader = true;
                })
                //.AddAuthorization()
                .AddApiExplorer()
                //.AddCors()
                .AddControllersAsServices();
            services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                AppDomain.CurrentDomain.Load("ArbitralSystem.Distributor.ScheduleDistributor.Domain"),
                AppDomain.CurrentDomain.Load("ArbitralSystem.Distributor.ScheduleDistributor.Persistence"));
            
            services.AddMediatR(AppDomain.CurrentDomain.Load("ArbitralSystem.Distributor.ScheduleDistributor.Domain"),
                AppDomain.CurrentDomain.Load("ArbitralSystem.Distributor.ScheduleDistributor.Persistence"));
            
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            
            });
            
            services.AddControllers();
            services.AddVersionedApiExplorer();
            services.AddArbitralSystemDbContext(distributorManagerOptions.DatabaseConnection);
            services.AddArbitralSystemSwagger("Distributor manager");
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(distributorManagerOptions.DatabaseConnection);
            });


            services.AddScoped<IJobManager, JobManagerRepository>();
            services.AddScoped<OrderBookDistributorDomainService>();
            services.AddScoped<PairInfoService>();
            services.AddOrderBookDistributorStub();
            services.AddPublicMarketInfo(marketInfoConnectionInfo);
            services.AddScoped<IDistributorRepository,DistributorRepository>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
            
            //app.UseHttpsRedirection();
            //app.UseRouting();
            //app.UseAuthorization();
            //pp.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseMvc();
            
            MigrateDbContext(app);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new [] { new HangFireAuthorizationFilter() }
            });
        }
        
        private static void MigrateDbContext(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DistributorDbContext>())
                {
                    context.Database.Migrate();
                    context.SaveChanges();
                }
            }
        }
    }
}