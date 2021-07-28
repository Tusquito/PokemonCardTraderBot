using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Bot;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using PokemonCardTraderBot.Common.Configurations;
using PokemonCardTraderBot.Common.Enums;
using PokemonCardTraderBot.Common.Extensions;
using PokemonCardTraderBot.Common.Services;
using PokemonCardTraderBot.Common.Views;
using PokemonCardTraderBot.Core.Managers;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;
using Qmmands;

namespace PokemonCardTraderBot.Core.Commands
{
    public class TestCommands : DiscordGuildModuleBase
    {
        private readonly SetManager _setManager;
        private readonly CardManager _cardManager;
        private readonly IRandomService _randomService;
        private readonly RaritiesConfiguration _configuration;

        public TestCommands(SetManager setManager, CardManager cardManager, IRandomService randomService, RaritiesConfiguration configuration)
        {
            _setManager = setManager;
            _cardManager = cardManager;
            _randomService = randomService;
            _configuration = configuration;
        }

        [Command("test-butttons"), Description("Test buttons command")]
        public DiscordCommandResult OnClickerTestCommand()
            => View(new ClickerView());

        [Command("test-booster"), Description("Test booster command")]
        public async Task<DiscordCommandResult> OnBoosterTestCommand([Remainder] string setCode)
        {
            SetData setData = await _setManager.SearchAsync(setCode);

            if (setData == null)
            {
                return Reply("Invalid set code");
            }

            List<PokemonCard> setCards = await _cardManager.GetOrAddBySetCode(setData.Code);
            
            CustomPagedView view = new CustomPagedView(new ListPageProvider(setCards.ToBooster(_randomService, _configuration)
                .ToPages(setData)));
            return Menu(new InteractiveMenu(Context.Author.Id, view));
        }
        
        [Command("test"), Description("Test rarities command")]
        public async Task<DiscordCommandResult> OnCardRaritiesCommand()
        {
            
            return Reply((await Card.AllAsync()).Select(x => x.SubType).ToHashSet().Aggregate((x,y) => $"{x}, {y}"));
        }
    }
}