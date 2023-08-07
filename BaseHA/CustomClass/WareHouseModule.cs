using Autofac;
using Autofac.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Behaviors;
using Share.BaseCore.Repositories;
using System;
using Nest;
using Share.BaseCore.IRepositories;
using Share.BaseCore.CustomConfiguration;
using Share.BaseCore.Base;
using Share.BaseCore.Extensions;
using BaseHA.Domain.ContextSub;

namespace BaseHA.CustomClass
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
            builder.AddDbContext<WarehouseManagementContext>(DataConnectionHelper.ConnectionStringNames.Warehouse);
            builder.AddRegisterDbContext(true);           
            // ef core
            builder.AddGeneric(DataConnectionHelper.ConnectionStringNames.Warehouse, DataConnectionHelper.ParameterName);

        }
    }

}