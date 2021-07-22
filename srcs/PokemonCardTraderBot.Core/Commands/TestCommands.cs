using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace PokemonCardTraderBot.Core.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test"), Description("Test command")]
        public async Task OnTestCommand(CommandContext ctx)
        {
            await ctx.RespondAsync("Some test command");
        }
    }
}