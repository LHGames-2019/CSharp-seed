﻿using LHGames.Helper;
using LHGames.Services;
using LHGames.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LHGames
{
    /// <summary>
    /// !!! DO NOT EDIT !!!
    /// </summary>
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<LHApiSignalrService>();
            services.AddSingleton<GameServerSignalrService>();

            IConfigurationSection appSettings = Configuration.GetSection("OfflineSettings");
            services.Configure<AppSettings>(appSettings);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            bool isOnline = Environment.GetEnvironmentVariable("LHAPI_URL") != "" && Environment.GetEnvironmentVariable("LHAPI_URL") != null;

            if (isOnline)
            {
                await app.ApplicationServices.GetService<LHApiSignalrService>().ConnectAsync();
            }
            else
            {
                await app.ApplicationServices.GetService<GameServerSignalrService>().ConnectAsync();
            }

        }
    }
}
