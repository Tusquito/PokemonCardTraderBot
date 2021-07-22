using System.Collections;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonCardTraderBot.Common.Options;

namespace PokemonCardTraderBot.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDiscordClient(this IServiceCollection services, IConfiguration configuration)
        {
            var botOptions = configuration.GetSection(DiscordBotOptions.Name).Get<DiscordBotOptions>();
            var client = new DiscordClient(new DiscordConfiguration
            {
                Token = botOptions.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Information
            });
            services.Configure<DiscordBotOptions>(configuration, x => x.BindNonPublicProperties = true);
            services.AddSingleton(client);
            services.AddSingleton(x =>
                client.UseCommandsNext(new CommandsNextConfiguration
                {
                    StringPrefixes = new[] { botOptions.Prefix },
                    EnableDms = true,
                    EnableMentionPrefix = true,
                    CaseSensitive = false,
                    IgnoreExtraArguments = true,
                    Services = x
                }));
        }
    }
}