using Autofac;
using Autofac.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using Serilog;
using BaseHA.Core.Authozire;
using BaseHA.Core.Extensions;
using BaseHA.Core.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using BaseHA.Core.IRepositories;
using BaseHA.Core.Base;
using BaseHA.Core.Enums;
using BaseHA.Core.CustomConfiguration;
using BaseHA.Core.Repositories;

namespace BaseHA.Core.CustomConfiguration
{
    public static class CustomConfigurationCore
    {
        public static void AddCustomConfigurationCore<TDbContext>(this IServiceCollection services, IConfiguration configuration, string nameConnect, DbType dbType = DbType.MSSQL) where TDbContext : DbContext
        {
            var sqlConnect = configuration.GetConnectionString(nameConnect);
            services.AddDbContext<TDbContext>(options =>
            {
                switch (dbType)
                {
                    case DbType.MSSQL:
                        options.UseSqlServer(sqlConnect,
                  sqlServerOptionsAction: sqlOptions =>
                  {
                      sqlOptions.MigrationsAssembly("sql MigrationsAssembly");
                  });
                        break;
                    case DbType.Oracle:
                        options.UseOracle(sqlConnect,
                       oracleOptionsAction: sqlOptions =>
                       {
                           sqlOptions.MigrationsAssembly("sql MigrationsAssembly");
                       });
                        break;
                    default:
                        break;
                }

                options.LogTo(Log.Information, LogLevel.Information, DbContextLoggerOptions.UtcTime).EnableSensitiveDataLogging();
            });
            // Register dynamic dbContext
            services.AddScoped<DbContext, TDbContext>();
        }

        public static void AddGeneric(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryEF<>), typeof(RepositoryEF<>));
        }

        public static void AddGeneric(this ContainerBuilder builder, string ConnectionStringNames, string ParameterName)
        {
            builder.RegisterGeneric(typeof(RepositoryEF<>))
       .Named(ConnectionStringNames, typeof(IRepositoryEF<>))
       .WithParameter(new ResolvedParameter(
           // kiểu truyền vào và tên biến truyền vào qua hàm khởi tạo
           (pi, ctx) => pi.ParameterType == typeof(DbContext) && pi.Name == ParameterName,
           (pi, ctx) => EngineContext.Current.Resolve<DbContext>(ConnectionStringNames))).InstancePerLifetimeScope();
        }

        /// <summary>
        /// Register resolving delegate DbContext
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="Lazy"></param>
        public static void AddRegisterDbContext(this ContainerBuilder builder, bool Lazy)
        {
            builder.Register<Func<string, DbContext>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return connectionStringName => cc.ResolveNamed<DbContext>(connectionStringName);
            });
            if (Lazy)
                builder.Register<Func<string, Lazy<DbContext>>>(c =>
                {
                    var cc = c.Resolve<IComponentContext>();
                    return connectionStringName => cc.ResolveNamed<Lazy<DbContext>>(connectionStringName);
                });
        }

        /// <summary>
        /// mulplite connect to dbcontext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="ConnectionStringNames"></param>
        public static void AddDbContext<TDbContext>(this ContainerBuilder builder, string ConnectionStringNames) where TDbContext : DbContext
        {
            builder.RegisterType<TDbContext>().Named<DbContext>(ConnectionStringNames).InstancePerDependency();
        }


        public static void AddSwaggerCore(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }
    }
}



