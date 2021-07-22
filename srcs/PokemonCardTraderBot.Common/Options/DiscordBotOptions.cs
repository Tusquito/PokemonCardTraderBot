namespace PokemonCardTraderBot.Common.Options
{
    public class DiscordBotOptions
    {
        public static string Name = nameof(DiscordBotOptions);
        public string Token { get; set; }
        public string Prefix { get; set; }
    }
}