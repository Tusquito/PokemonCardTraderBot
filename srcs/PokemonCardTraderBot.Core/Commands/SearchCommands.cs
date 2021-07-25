using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using PokemonCardTraderBot.Common.Extensions;
using PokemonCardTraderBot.Common.Views;
using PokemonCardTraderBot.Core.Managers;
using PokemonTcgSdk.Models;
using Qmmands;

namespace PokemonCardTraderBot.Core.Commands
{
    public class SearchCommands : DiscordGuildModuleBase
    {
        private readonly ILogger<SearchCommands> _logger;
        private readonly SetManager _setManager;
        private readonly CardManager _cardManager;

        public SearchCommands(ILogger<SearchCommands> logger, SetManager setManager, CardManager cardManager)
        {
            _logger = logger;
            _setManager = setManager;
            _cardManager = cardManager;
        }
        
        
       [Command("list-sets"), Description("Search sets command")]
        public async Task<DiscordCommandResult> OnSearchSetsCommand()
        {
            List<SetData> result = await _setManager.GetOrAddAllAsync();
            
            if (result == null || !result.Any())
            {
                return Reply(new LocalMessage().WithContent("No result found"));
            }

            var view = new CustomPagedView(new ListPageProvider(result.GroupBy(x => x.Series)
                .ToDictionary(x => x.Key, x => x.ToList())
                .ToPages()));
            return Menu(new InteractiveMenu(Context.Author.Id, view));
        }

        [Command("search-set"), Description("Search set command")]
        public async Task OnSearchSetCommand([Remainder]string search)
        {
            SetData result = await _setManager.SearchAsync(search);

            if (result == null)
            {
                await Context.Channel.SendMessageAsync(new LocalMessage().WithContent("No result found"));
                return;
            }

            await Context.Channel.SendMessageAsync(new LocalMessage().WithEmbeds(new List<LocalEmbed>
            {
                new ()
                {
                    Color = Color.Aquamarine,
                    ThumbnailUrl = result.LogoUrl,
                    Title = result.Name,
                    Description = result
                        .GetType()
                        .GetProperties()
                        .Aggregate("", (current, prop) => current + $"**{prop.Name}:** {prop.GetValue(result)}\n"),
                    Timestamp = DateTimeOffset.UtcNow
                }
            }));
        }
        
        [Command("search-set-cards"), Description("Search set cards command")]
        public async Task OnSearchSetCardsCommand([Remainder]string setCode)
        {
            SetData setData = await _setManager.GetBySetCodeAsync(setCode);
            if (setData == null)
            {
                await Context.Channel.SendMessageAsync(new LocalMessage().WithContent("Invalid set code"));
                return;
            }
            
            List<PokemonCard> result = await _cardManager.GetOrAddBySetCode(setCode);

            if (result == null || !result.Any())
            {
                await Context.Channel.SendMessageAsync(new LocalMessage().WithContent("No result found"));
                return;
            }

            string rarities = result.GroupBy(x => x.Rarity)
                .Select(group => new {Rarity = group.Key, Count = group.Count()})
                .OrderBy(x => x.Count)
                .Select(x => $"{x.Rarity} ({x.Count})")
                .Aggregate((x, y) => $"{x}, {y}");

            await Context.Channel.SendMessageAsync(new LocalMessage().WithEmbeds(new List<LocalEmbed>
            {
                new()
                {
                    Color = Color.Aquamarine,
                    ThumbnailUrl = setData.LogoUrl,
                    Title = $"{setData.Name} cards",
                    Description = $"**Rarities:** {rarities}\n**Amount:** {result.Count}",
                    Timestamp = DateTimeOffset.UtcNow
                }
            }));
        }
    }
}