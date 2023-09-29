using Autofac;
using Autofac.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BaseHA.Core.Behaviors;
using BaseHA.Core.Repositories;
using System;
using Nest;
using BaseHA.Core.IRepositories;
using BaseHA.Domain.Contexts;
using BaseHA.Core.Base;
using BaseHA.Core.Extensions;
using BaseHA.Core.CustomConfiguration;

namespace BaseHA.Application.ModuleAutoFac
{
    /// <summary>
    /// app.ConfigureRequestPipeline(); đăng ký
    /// </summary>
    public class WareHouseModule : Module
    {
        public WareHouseModule()
        {
        }
        protected override void Load(ContainerBuilder builder)
        {
            // Declare your services with proper lifetime
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            builder.RegisterType<BaseEngine>().As<IEngine>().SingleInstance();
            //
            builder.AddDbContext<WareHouseContext>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            builder.AddRegisterDbContext(true);
            // ef core
            builder.AddGeneric(DataConnectionHelper.ConnectionStringNames.Warehouse, DataConnectionHelper.ParameterName);

        }
    }

}