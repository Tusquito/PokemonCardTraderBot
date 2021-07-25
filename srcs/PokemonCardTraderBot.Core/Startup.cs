using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PokemonCardTraderBot.Common.Services;
using PokemonCardTraderBot.Core.Managers;
using PokemonCardTraderBot.Core.Services;
using Qmmands;

namespace PokemonCardTraderBot.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<SetManager>();
            services.AddSingleton<CardManager>();
            services.AddSingleton<IRandomService, RandomService>();
        }
        
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
        }
    }
}