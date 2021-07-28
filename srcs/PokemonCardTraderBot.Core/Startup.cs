using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PokemonCardTraderBot.Common.Extensions;
using PokemonCardTraderBot.Common.Services;
using PokemonCardTraderBot.Core.Managers;
using PokemonCardTraderBot.Core.Services;

namespace PokemonCardTraderBot.Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddConfigurationFiles();
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