using MediatR;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Share.BaseCore.CustomConfiguration;
using BaseHA.Controllers;
using BaseHA.Infrastructure;
using Serilog;
using Newtonsoft.Json.Serialization;
using Share.BaseCore.Filters;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BaseHA.Configuration
{
    public static class CustomConfiguration
    {
        public static void AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomConfigurationCoreDB<FakeDbContext>(configuration, "", true);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(CustomValidationAttribute));
            })
              // .AddApplicationPart(typeof(HomeController).Assembly)
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.WriteIndented = true;
                   options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
               }).AddNewtonsoftJson(options =>
                      options.SerializerSettings.ContractResolver =
                         new DefaultContractResolver());

        }
        public static void AddCustomConfigurationCoreDB<TDbContext>(this IServiceCollection services, IConfiguration configuration, string nameConnect, bool isDbMemory) where TDbContext : DbContext
        {

            var sqlConnect = configuration.GetConnectionString(nameConnect);
            services.AddDbContextPool<TDbContext>(options =>
            {
                options.UseInMemoryDatabase(typeof(TDbContext).ToString());
                options.LogTo(Log.Information, Microsoft.Extensions.Logging.LogLevel.Information, Microsoft.EntityFrameworkCore.Diagnostics.DbContextLoggerOptions.UtcTime).ConfigureWarnings(
            b => b.Log(
                (RelationalEventId.ConnectionOpened, LogLevel.Information),
                (RelationalEventId.ConnectionClosed, LogLevel.Information))).EnableSensitiveDataLogging().EnableDetailedErrors().EnableThreadSafetyChecks().EnableServiceProviderCaching();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<DbContext, TDbContext>();


        }
    }
}
