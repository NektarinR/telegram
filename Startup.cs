using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ether_bot.Services;
using Ether_bot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ether_bot.Interfaces;
using Ether_bot.Context;

namespace Ether_bot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<BotSettings>(Configuration.GetSection("BotSettings").GetSection("EthereumBot"));
            services.Configure<ExchangeSettings>(Configuration.GetSection("ExchangeSettings"));
            services.AddSingleton<IBotService, EthereumBotService>();
            services.AddSingleton<IExchangeService, ExchangeService>();
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IStorageService, SqliteStorageService>();
            services.AddMemoryCache();
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<EthereumBotContext>(options => 
                options.UseLazyLoadingProxies().UseSqlite(Configuration.GetConnectionString("SqliteConnection"))
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}