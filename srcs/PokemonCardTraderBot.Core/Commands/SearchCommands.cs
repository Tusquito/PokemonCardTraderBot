using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Rest;
using Microsoft.Extensions.Logging;
using PokemonCardTraderBot.Core.Managers;
using PokemonTcgSdk.Models;
using Qmmands;

namespace PokemonCardTraderBot.Core.Commands
{
    public class SearchCommands : DiscordGuildModuleBase
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
        
        
       [Command("search-sets"), Description("Search sets command")]
        public async Task OnSearchSetsCommand()
        {
            List<SetData> result = await _setsManager.GetOrAddAllAsync();

            if (result == null || !result.Any())
            {
                await Context.Channel.SendMessageAsync(new LocalMessage {Content = "No result found"});
                return;
            }

            var desc = result.GroupBy(x => x.Series)
                .Select(group =>
                    new
                    {
                        Series = group.Key,
                        Names = group.Select(x => $"**[{x.Code}]** {x.Name}").Aggregate((x, y) => $"{x}\n{y}")
                    })
                .OrderBy(x => x.Series)
                .Select(x => $"__{x.Series}__: \n{x.Names}")
                .Aggregate((x, y) => $"{x}\n{y}");
        }

        [Command("search-set"), Description("Search set command")]
        public async Task OnSearchSetCommand([Remainder]string name)
        {
            SetData result = await _setsManager.GetByNameAsync(name);

            if (result == null)
            {
                await Context.Channel.SendMessageAsync(new LocalMessage {Content = "No result found"});
                return;
            }

            await Context.Channel.SendMessageAsync(new LocalMessage
            {
                Embeds = new List<LocalEmbed>
                {
                    new()
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
                }
            });
        }
        
        [Command("search-set-cards"), Description("Search set cards command")]
        public async Task OnSearchSetCardsCommand([Remainder]string setCode)
        {
            SetData setData = await _setsManager.GetBySetCodeAsync(setCode);
            if (setData == null)
            {
                await Context.Channel.SendMessageAsync(new LocalMessage {Content = "Invalid set code"});
                return;
            }
            
            List<PokemonCard> result = await _cardsManager.GetOrAddBySetCode(setCode);

            if (result == null || !result.Any())
            {
                await Context.Channel.SendMessageAsync(new LocalMessage {Content = "No result found"});
                return;
            }

            string rarities = result.GroupBy(x => x.Rarity)
                .Select(group => new {Rarity = group.Key, Count = group.Count()})
                .OrderBy(x => x.Count)
                .Select(x => $"{x.Rarity} ({x.Count})")
                .Aggregate((x, y) => $"{x}, {y}");

            await Context.Channel.SendMessageAsync(new LocalMessage
            {
                Embeds = new List<LocalEmbed>
                {
                    new()
                    {
                        Color = Color.Aquamarine,
                        ThumbnailUrl = setData.LogoUrl,
                        Title = $"{setData.Name} cards",
                        Description = $"**Rarities:** {rarities}\n**Amount:** {result.Count}",
                        Timestamp = DateTimeOffset.UtcNow
                    }
                }
            });
        }
    }
}