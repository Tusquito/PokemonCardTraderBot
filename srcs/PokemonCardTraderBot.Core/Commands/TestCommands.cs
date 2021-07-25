using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord;
using Disqord.Bot;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using PokemonCardTraderBot.Common.Extensions;
using PokemonCardTraderBot.Common.Services;
using PokemonCardTraderBot.Common.Views;
using PokemonCardTraderBot.Core.Managers;
using PokemonTcgSdk.Models;
using Qmmands;

namespace PokemonCardTraderBot.Core.Commands
{
    public class TestCommands : DiscordGuildModuleBase
    {
        private readonly SetManager _setManager;
        private readonly CardManager _cardManager;
        private readonly IRandomService _randomService;

        public TestCommands(SetManager setManager, CardManager cardManager, IRandomService randomService)
        {
            _setManager = setManager;
            _cardManager = cardManager;
            _randomService = randomService;
        }

        [Command("test-butttons"), Description("Test buttons command")]
        public async Task<DiscordCommandResult> OnClickerTestCommand()
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
            
            CustomPagedView view = new CustomPagedView(new ListPageProvider(setCards.ToBooster(_randomService).ToPages(setData)));
            return Menu(new InteractiveMenu(Context.Author.Id, view));
        }
            
    }
}