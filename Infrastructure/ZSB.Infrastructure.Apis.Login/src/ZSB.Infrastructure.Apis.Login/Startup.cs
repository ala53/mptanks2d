﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Configuration;
using Microsoft.Dnx.Runtime;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.Diagnostics;

namespace ZSB.Infrastructure.Apis.Login
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(a =>
                a.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver());
            services.AddCors();
            
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<Database.Contexts.LoginDatabaseContext>(o =>
                {
                    o.UseSqlServer(Configuration["Data:ConnectionString"]);
                });
            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseWelcomePage();
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
#if DEBUG
#endif
            app.Properties.Add("host.AppMode", "development");
            app.UseErrorPage();

            app.UseCors(a => a.AllowAnyMethod().AllowAnyHeader().WithOrigins("*.zsbgames.me", "localhost"));
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
        }
    }
}