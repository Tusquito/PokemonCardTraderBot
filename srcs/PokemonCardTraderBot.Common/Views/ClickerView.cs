using System.Threading.Tasks;
using Disqord;
using Disqord.Extensions.Interactivity.Menus;

namespace PokemonCardTraderBot.Common.Views
{
    public class ClickerView : ViewBase
    {
        private int _clicks;
        
        public ClickerView() : base(new LocalMessage().WithContent("Click the button!"))
        {
        }
        
        [Button(Label = "Click Me")]
        public ValueTask ClickMe(ButtonEventArgs e)
        {
            e.Button.Label = $"{++_clicks} clicks";
            return default;
        }
        
        [Button(Label = "Test")]
        public ValueTask Test(ButtonEventArgs e)
        {
            e.Button.Style = LocalButtonComponentStyle.Danger;
            return default;
        }
        
        
    }
}