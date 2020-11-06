using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.CoinEx;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.PublicMarketInfoService.Common.Auth;
using ArbitralSystem.PublicMarketInfoService.Domain.Interfaces;
using ArbitralSystem.PublicMarketInfoService.Domain.Services;
using ArbitralSystem.PublicMarketInfoService.Extensions;
using ArbitralSystem.PublicMarketInfoService.Persistence;
using ArbitralSystem.PublicMarketInfoService.Persistence.Repositories;
using ArbitralSystem.PublicMarketInfoService.Services;
using ArbitralSystem.PublicMarketInfoService.Workflow;
using AutoMapper;
using Hangfire;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace ArbitralSystem.PublicMarketInfoService
{
    [UsedImplicitly]
    public class Startup 
    {
        private readonly IConfigurationRoot _configuration;
        private const string DomainLayer = "ArbitralSystem.PublicMarketInfoService.Domain";
        private const string PersistenceLayer = "ArbitralSystem.PublicMarketInfoService.Persistence";
        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(config =>
                {
                    config.EnableEndpointRouting = false;
                    config.RespectBrowserAcceptHeader = true;
                })
                //.AddAuthorization()
                .AddApiExplorer()
                .AddCors()
                .AddControllersAsServices();
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                AppDomain.CurrentDomain.Load(DomainLayer),
                AppDomain.CurrentDomain.Load(PersistenceLayer));
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            
            });
            
            services.AddControllers();
            services.AddVersionedApiExplorer();
            services.AddArbitralSystemDbContext(_configuration);
            services.AddArbitralSystemSwagger("Public market-info service");
            services.AddHangfire(configuration => configuration
                .UseSqlServerStorage(_configuration["Data:DefaultConnection:ConnectionString"]));
            
            services.AddArbitralSystemServices(_configuration);
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new [] { new HangFireAuthorizationFilter() }
            });
            app.UseHangfireServer();
            
            RecurringJob.AddOrUpdate<PairInfoUpdaterJob>("Pair-info-update", x => x.Execute(), "0 0 12 * * ?" );// every noon at 12 
            
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
            app.UseCors(builder => builder.AllowAnyOrigin());
            //app.UseHttpsRedirection();
            app.UseMvc();
            
            MigrateDbContext(app);
        }
        
        private static void MigrateDbContext(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<PublicMarketInfoBdContext>())
                {
                    context.Database.Migrate();
                    context.SaveChanges();
                }
            }
        }
    }
}