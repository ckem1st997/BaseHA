
using BaseHA.Application.ModelDto;
using BaseHA.Application.Validations.BaseValidator;
using BaseHA.Application.Validations.Unit;
using BaseHA.Application.Validations.WareHouse;
using BaseHA.Models.SearchModel;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace BaseHA.Application.Validations.ConfigureServices
{
    public static class ServiceValidator
    {
        public static void AddValidator(this IServiceCollection services)
        {
            services.AddFluentValidation();
            services.AddTransient<IValidator<WareHouseCommands>, WareHouseCommandValidator>();

            services.AddTransient<IValidator<UnitCommands>, UnitCommandValidator>();

            services.AddTransient<IValidator<VendorCommands>, VendorCommandValidator>();


            services.AddScoped<IValidator<BaseSearchModel>, BaseSearchModelValidator>();

        }
    }
}