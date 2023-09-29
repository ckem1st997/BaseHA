using BaseHA.Application.AutoMapper.WareHouses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseHA.Application.AutoMapper.ConfigureServices
{
    public static class ServiceMapper
    {
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(WareHouseCommandProfile).Assembly);
            services.AddAutoMapper(typeof(BeginningWareHouseProfile).Assembly);
        }
    }
}