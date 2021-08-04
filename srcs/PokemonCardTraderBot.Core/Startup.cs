using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonCardTraderBot.Common.Extensions;
using PokemonCardTraderBot.Common.Services;
using PokemonCardTraderBot.Core.Managers;
using PokemonCardTraderBot.Core.Services;
using PokemonCardTraderBot.Database.Extensions;

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
            services.AddMemoryCache();
            services.AddConfigurationFiles(_configuration);
            //services.AddDatabaseContext(_configuration);
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