using System.Collections.Generic;

namespace PokemonCardTraderBot.Common.Options
{
    public class DiscordBotOptions 
    {
        public const string Name = nameof(DiscordBotOptions);
        public string Token { get; set; }
        public List<string> Prefixes { get; set; }
    }
}