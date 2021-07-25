using Disqord;
using Disqord.Extensions.Interactivity.Menus.Paged;

namespace PokemonCardTraderBot.Common.Views
{
    public class CustomPagedView : PagedView
    {
        public CustomPagedView(PageProvider pageProvider, LocalMessage templateMessage = null) : base(pageProvider, templateMessage)
        {
            FirstPageButton.Label = "First";
            FirstPageButton.Emoji = null; 
            LastPageButton.Label = "Last";
            LastPageButton.Emoji = null;
            NextPageButton.Label = "Next";
            NextPageButton.Emoji = null;
            PreviousPageButton.Label = "Previous";
            PreviousPageButton.Emoji = null;
            StopButton.Label = "Finish";
            StopButton.Emoji = null;
        }
    }
}