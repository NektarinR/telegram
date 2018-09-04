using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ether_bot.Services;
using Ether_bot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
            services.AddSingleton<IBotService, EthereumBotService>();
            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IStorageService, SqliteStorageService>();
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<EthereumBotContext>(options => 
                options.UseLazyLoadingProxies().UseSqlite(Configuration.GetConnectionString("SqliteConnection"))
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IBotService botService)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}