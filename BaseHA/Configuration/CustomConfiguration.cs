using MediatR;
using Share.BaseCore.Specification;
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

namespace BaseHA.Configuration
{
    public static class CustomConfiguration
    {
        public static void AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomConfigurationCore<HomeController>(configuration, "Test");
            services.AddCustomConfigurationCoreDB<FakeDbContext>(configuration, "", true);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddGenericRepository<FakeDbContext>();
           // services.AddGenericRepository<Fake2DbContext>();
          //services.AddCustomConfigurationCoreDapper();

        }

    }
}
