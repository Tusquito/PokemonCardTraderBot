using System.Collections.Generic;
using PokemonCardTraderBot.Common.Attributes;

namespace PokemonCardTraderBot.Common.Configurations
{
    [ConfigurationFileName("card_sets_configuration")]
    public class CardSetsConfiguration : Dictionary<string, CardSetInfo>, IDefaultConfiguration { }
    
    public class CardSetInfo
    {
        public string Code { get; set; }
        public bool IsBlacklisted { get; set; } = false;
    }
}