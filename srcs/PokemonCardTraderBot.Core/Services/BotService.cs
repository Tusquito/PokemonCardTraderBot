using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Hosting;
using PokemonCardTraderBot.Core.Commands;

namespace PokemonCardTraderBot.Core.Services
{
    public class BotService : BackgroundService
    {
        private readonly DiscordClient _client;
        private readonly CommandsNextExtension _commands;

        public BotService(DiscordClient client, CommandsNextExtension commands)
        {
            _client = client;
            _commands = commands;

            _commands.CommandErrored += OnCommandErrored;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _commands.RegisterCommands<TestCommands>();
            _client.Ready += ClientOnReady;
            await _client.ConnectAsync();
        }
        

        private Task ClientOnReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        private static Task OnCommandErrored(CommandsNextExtension commandNext, CommandErrorEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
