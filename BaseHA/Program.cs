using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BaseHA.Core;
using BaseHA.Core.Cache;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Grpc.Net.Client.Web;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BaseHA.Core.Authozire;
using BaseHA.Core.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using BaseHA.Core.CustomConfiguration;
using System.Configuration;
using System.Reflection;
using BaseHA.Application.Configuration;
using BaseHA.Application.ModuleAutoFac;
using BaseHA.Application.AutoMapper.ConfigureServices;
using BaseHA.Application.Validations.ConfigureServices;
using BaseHA.Application.AutoMapper.WareHouses;
using AutoMapper;
using BaseHA.Core.Authozire.ConfigureServices;
using BaseHA.Core.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
var Configuration = builder.Configuration;
//
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production)
{
    var redis = ConnectionMultiplexer.Connect("192.168.3.130:6379");
    services.AddDataProtection().SetApplicationName("Base")
        .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
        {
            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
        }).PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
}

else
    services.AddDataProtection().SetApplicationName("Base")
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });
services.AddAntiforgery();

services.AddMapper();
services.AddMvc();
services.AddValidator();
//
services.AddCustomDI();
//
services.AddMediatR(Assembly.GetExecutingAssembly());
//services.AddCache(Configuration);
services.AddCustomConfiguration(Configuration);
// Add the Kendo UI services to the services container.
services.AddKendo();
//services.AddHostedService<RequestTimeConsumer>();
// call http to grpc
services.Configure<PasswordHasherOptions>(option =>
{
    option.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
    option.IterationCount = 12000;
});
services.AddApiAuthentication();
services.AddApiCors();
//auto fac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(
   builder => builder.RegisterModule(new WareHouseModule()));


var app = builder.Build();

//auto fac
ILifetimeScope AutofacContainer = app.Services.GetAutofacRoot();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// migrate any database changes on startup (includes initial db creation)

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.ConfigureRequestPipeline();
await app.RunAsync();
