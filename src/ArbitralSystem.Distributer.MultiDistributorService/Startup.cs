using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ArbitralSystem.Connectors.ArbitralPublicMarketInfoConnector;
using ArbitralSystem.Connectors.Core;
using ArbitralSystem.Connectors.Core.Arbitral;
using ArbitralSystem.Connectors.Core.Converters;
using ArbitralSystem.Connectors.Core.Distributers;
using ArbitralSystem.Connectors.CryptoExchange;
using ArbitralSystem.Connectors.CryptoExchange.Common;
using ArbitralSystem.Connectors.CryptoExchange.Converter;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Interfaces;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Jobs;
using ArbitralSystem.Distributer.OrderBookDistributerDomain.Models;
using ArbitralSystem.Distributer.OrderBookDistributorCore.Messaging;
using ArbitralSystem.Distributer.OrderBookMultiDistributorDomain.Services;
using ArbitralSystem.Distributer.OrderBookMultiDistributorService.Extensions;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace ArbitralSystem.Distributer.OrderBookMultiDistributorService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
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
                //.AddCors()
                .AddControllersAsServices();
            
            //services.AddAutoMapper(Assembly.GetExecutingAssembly(),
            //    AppDomain.CurrentDomain.Load("ArbitralSystem.PublicMarketInfoService.Domain"),
            //    AppDomain.CurrentDomain.Load("ArbitralSystem.PublicMarketInfoService.Persistence"));
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            
            });
            services.AddControllers();
            services.AddVersionedApiExplorer();
           
            services.AddArbitralSystemSwagger("Multi order-book distributor service service");
            services.AddHangfire(configuration => configuration
                .UseSqlServerStorage(_configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddScoped<DistributorManagerDomainService>();
            services.AddScoped<IPublicMarketInfoConnector, PublicMarketInfoConnector>();
            
            IExchangeConnectionInfo[] connectionInfoArray =
                {_configuration.GetSection("Connectors:CoinEx").Get<ExchangeConnectionInfo>()};

            IConnectionInfo connectionInfo = _configuration.GetSection("Connectors:asd").Get<ExchangeConnectionInfo>();
            services.AddSingleton(connectionInfoArray);
            services.AddSingleton(connectionInfo);

            services.AddScoped<DistributorJob>();
            services.AddScoped<IOrderBookDistributerFactory,CryptoExOrderBookDistributerFactory>();
            services.AddSingleton<IDtoConverter, CryptoExchangeConverter>();
            IDistributerOptions opt = new DistributerOptions();
            services.AddSingleton(opt);
            var a = new DistributionOptions(10);
            services.AddSingleton(a);

            services.AddScoped<IOrderBookPublisher, OrderBookPublisher>();


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //AppDomain.CurrentDomain.Load("ArbitralSystem.PublicMarketInfoService.Domain"),
            //AppDomain.CurrentDomain.Load("ArbitralSystem.PublicMarketInfoService.Persistence"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            
            //RecurringJob.AddOrUpdate<PairInfoUpdaterJob>("Pair-info-update", x => x.Execute(), "0 0 12 * * ?" );// every noon at 12 
            
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
            
            app.UseHttpsRedirection();
            //app.UseRouting();
            //app.UseAuthorization();
            //pp.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseMvc();
            
            //MigrateDbContext(app);
        }
    }
}