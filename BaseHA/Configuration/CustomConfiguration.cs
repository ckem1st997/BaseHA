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

namespace BaseHA.Configuration
{
    public static class CustomConfiguration
    {
        public static void AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomConfigurationCoreDB<FakeDbContext>(configuration, "", true);
            services.AddMediatR(Assembly.GetExecutingAssembly());
          //services.AddCustomConfigurationCoreDapper();

        }
        public static void AddCustomConfigurationCoreDB<TDbContext>(this IServiceCollection services, IConfiguration configuration, string nameConnect, bool isDbMemory) where TDbContext : DbContext
        {

            var sqlConnect = configuration.GetConnectionString(nameConnect);
            Console.WriteLine("Set up Sql connect");
            Console.WriteLine(sqlConnect);
            if (!isDbMemory)
                services.AddDbContextPool<TDbContext>(options =>
                {
                    options.UseSqlServer(sqlConnect,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            //  sqlOptions.MigrationsAssembly(typeof(TStartUp).GetTypeInfo().Assembly.GetName().Name);
                            // sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                    options.LogTo(Log.Information, Microsoft.Extensions.Logging.LogLevel.Information, Microsoft.EntityFrameworkCore.Diagnostics.DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
                );
            else
                services.AddDbContextPool<TDbContext>(options =>
                {
                    options.UseInMemoryDatabase(typeof(TDbContext).ToString());
                    options.LogTo(Log.Information, Microsoft.Extensions.Logging.LogLevel.Information, Microsoft.EntityFrameworkCore.Diagnostics.DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

            services.AddScoped<DbContext, TDbContext>();


        }
    }
}
