using BaseHA.Configuration;
using BaseHA.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Share.BaseCore.Kafka;
using Share.BaseCore.Cache;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Grpc.Net.Client.Web;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
using Share.BaseCore.Authozire;
using Share.BaseCore.Authozire.ConfigureServices;
using Share.BaseCore.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;















var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
var Configuration = builder.Configuration;



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
//services.AddSignalR(options =>
//{
//    // Global filters will run first
//    options.AddFilter<CustomFilter>();
//});

var app = builder.Build();




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


await app.RunAsync();
