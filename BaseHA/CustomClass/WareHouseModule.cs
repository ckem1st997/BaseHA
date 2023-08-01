using Autofac;
using Autofac.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Behaviors;
using Share.BaseCore.Repositories;
using System;
using BaseHA.Infrastructure;
using Nest;
using Share.BaseCore.IRepositories;
using Share.BaseCore.CustomConfiguration;
using Share.BaseCore.BaseNop;
using Share.BaseCore.Extensions;


namespace BaseHA.CustomClass
{
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

            //builder.AddDbContext<FakeDbContext>(DataConnectionHelper.ConnectionStringNames.Warehouse);


            //builder.AddRegisterDbContext(true);
            ////
            //// ef core

            //builder.AddGeneric(DataConnectionHelper.ConnectionStringNames.Warehouse, DataConnectionHelper.ParameterName);

        }
    }

}