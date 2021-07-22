using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonCardTraderBot.Common.Extensions;
using PokemonCardTraderBot.Core.Managers;
using PokemonCardTraderBot.Core.Services;

namespace PokemonCardTraderBot.Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDiscordClient(_configuration);
            services.AddHostedService<BotService>();
            services.AddMemoryCache();

            services.AddSingleton<SetsManager>();
            services.AddSingleton<CardsManager>();
        }
        
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
        }
    }
}