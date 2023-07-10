using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using Newtonsoft.Json.Serialization;
using Serilog;
using Share.BaseCore.Authozire;
using Share.BaseCore.Extensions;
using Share.BaseCore.Filters;
using Share.BaseCore.IRepositories;
using Share.BaseCore.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Share.BaseCore.CustomConfiguration
{
    public static class CustomConfigurationCore
    {
        public static void AddCustomConfigurationCore<TControler>(this IServiceCollection services, IConfiguration configuration, string TitleSwagger)
        {
            services.AddOptions();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(CustomValidationAttribute));
            })
                .AddApplicationPart(typeof(TControler).Assembly)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                }).AddNewtonsoftJson(options =>
                       options.SerializerSettings.ContractResolver =
                          new DefaultContractResolver());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = TitleSwagger, Version = "v1" });
            });
            services.AddScoped(typeof(IRepositoryEF<>), typeof(RepositoryEF<>));

        }      
        
        
        public static void AddCustomConfigurationCoreDapper(this IServiceCollection services)
        {

            services.AddScoped<IDapper, Dapperr>();
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

        }





        /// <summary>
        /// Add generic repository services to the .NET Dependency Injection container.
        /// </summary>
        /// <typeparam name="TDbContext">Your EF Core <see cref="DbContext"/>.</typeparam>
        /// <param name="services">The type to be extended.</param>
        /// <param name="lifetime">The life time of the service.</param>
        /// <returns>Retruns <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddGenericRepository<TDbContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IGenericRepository),
                serviceProvider =>
                {
                    TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    return new Repository<TDbContext>(dbContext);
                },
                lifetime));

            services.Add(new ServiceDescriptor(
               typeof(IGenericRepository<TDbContext>),
               serviceProvider =>
               {
                   TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                   return new Repository<TDbContext>(dbContext);
               },
               lifetime));

            return services;
        }




        /// <summary>
        /// Add generic query repository services to the .NET Dependency Injection container.
        /// </summary>
        /// <typeparam name="TDbContext">Your EF Core <see cref="DbContext"/>.</typeparam>
        /// <param name="services">The type to be extended.</param>
        /// <param name="lifetime">The life time of the service.</param>
        /// <returns>Retruns <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddQueryRepository<TDbContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IQueryRepository),
                serviceProvider =>
                {
                    TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    return new QueryRepository<TDbContext>(dbContext);
                },
                lifetime));

            services.Add(new ServiceDescriptor(
                typeof(IQueryRepository<TDbContext>),
                serviceProvider =>
                {
                    TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    return new QueryRepository<TDbContext>(dbContext);
                },
                lifetime));

            return services;
        }


    }
}



