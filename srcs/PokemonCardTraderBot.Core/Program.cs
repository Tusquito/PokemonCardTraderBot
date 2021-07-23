using System.Threading.Tasks;
using Disqord.Bot.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PokemonCardTraderBot.Core
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.StartAsync();
            await host.WaitForShutdownAsync();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging(s =>
                    {
                        s.AddFilter("Microsoft", LogLevel.Warning);
                    });

                    webBuilder.ConfigureKestrel(s =>
                    {
                        s.ListenAnyIP(37777);
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    bot.Token = context.Configuration["TOKEN"];
                    bot.Prefixes = new[] { context.Configuration["PREFIX"] };
                })
                .UseConsoleLifetime();
            return host;
        }
    }
}