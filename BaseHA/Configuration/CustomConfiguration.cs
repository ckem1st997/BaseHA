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
using Serilog;
using Newtonsoft.Json.Serialization;
using Share.BaseCore.Filters;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BaseHA.Domain.ContextSub;
using BaseHA.Serivce;

namespace BaseHA.Configuration
{
    public static class CustomConfiguration
    {
        public static void AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomConfigurationCore<WarehouseManagementContext>(configuration, "WarehouseManagementContext");
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
            //services.AddOptions();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(CustomValidationAttribute));
            })
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.WriteIndented = true;
                   options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
               }).AddNewtonsoftJson(options =>
                      options.SerializerSettings.ContractResolver =
                         new DefaultContractResolver());

        }


        public static void AddCustomDI(this IServiceCollection services)
        {
            services.AddScoped<IWareHouseService, WareHouseService>();
        }
    }
}
