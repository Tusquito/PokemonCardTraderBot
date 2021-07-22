using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using PokemonCardTraderBot.Core.Managers;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Core.Commands
{
    public class SearchCommands : BaseCommandModule
    {
        private readonly ILogger<SearchCommands> _logger;
        private readonly SetsManager _setsManager;
        private readonly CardsManager _cardsManager;

        public SearchCommands(ILogger<SearchCommands> logger, SetsManager setsManager, CardsManager cardsManager)
        {
            _logger = logger;
            _setsManager = setsManager;
            _cardsManager = cardsManager;
        }

        [Command("search-set"), Description("Search set command")]
        public async Task OnSearchSetCommand(CommandContext ctx, [RemainingText]string name)
        {
            SetData result = await _setsManager.GetByNameAsync(name);

            if (result == null)
            {
                await ctx.RespondAsync("No result found");
                return;
            }

            await ctx.RespondAsync(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Aquamarine)
                .WithThumbnail(result.LogoUrl)
                .WithTitle(result.Name)
                .WithDescription(result
                    .GetType()
                    .GetProperties()
                    .Aggregate("", (current, prop) => current + $"**{prop.Name}:** {prop.GetValue(result)}\n"))
                .WithTimestamp(DateTime.UtcNow)
            );
        }
        
        [Command("search-set-cards"), Description("Search set cards command")]
        public async Task OnSearchSetCardsCommand(CommandContext ctx, [RemainingText]string setCode)
        {
            SetData setData = await _setsManager.GetBySetCodeAsync(setCode);
            if (setData == null)
            {
                await ctx.RespondAsync("Invalid set code");
                return;
            }
            
            List<PokemonCard> result = await _cardsManager.GetOrAddBySetCode(setCode);

            if (result == null || !result.Any())
            {
                await ctx.RespondAsync("No result found");
                return;
            }

            string rarities = result.GroupBy(x => x.Rarity)
                .Select(group => new {Rarity = group.Key, Count = group.Count()})
                .OrderBy(x => x.Count)
                .Select(x => $"{x.Rarity} ({x.Count})")
                .Aggregate((x, y) => $"{x}, {y}");

            await ctx.RespondAsync(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Aquamarine)
                .WithThumbnail(setData.LogoUrl)
                .WithTitle($"{setData.Name} cards")
                .WithDescription($"**Rarities:** {rarities}\n" +
                                 $"**Amount:** {result.Count}")
                .WithTimestamp(DateTime.UtcNow)
            );
        }
        
    }
}