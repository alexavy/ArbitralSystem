using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DistributorManagementService.Extensions;
using DistributorManagementService.Extensions.Serialisation;
using DistributorManagementService.Persistence;
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
using Serilog;

namespace DistributorManagementService
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
            

            
            services.AddControllers().AddNewtonsoftJson(o=>DefaultJsonSerializerSettings.Apply(o.SerializerSettings));
            services.AddControllers();
            services.AddVersionedApiExplorer();
            services.AddArbitralSystemDbContext(_configuration);
            services.AddArbitralSystemSwagger("Distributor control service");
            services.AddDistributerControlServices(_configuration);
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                });
            
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseMvc();
            
            MigrateDbContext(app);
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