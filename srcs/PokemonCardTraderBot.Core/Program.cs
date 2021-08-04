using System.Threading.Tasks;
using Disqord.Bot.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PokemonCardTraderBot.Common.Options;

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
                    var botConfig = context.Configuration.GetSection(DiscordBotOptions.Name).Get<DiscordBotOptions>();
                    bot.Token = botConfig.Token;
                    bot.Prefixes = botConfig.Prefixes;
                    bot.UseMentionPrefix = true;
                })
                .UseConsoleLifetime();
            return host;
        }
    }
}