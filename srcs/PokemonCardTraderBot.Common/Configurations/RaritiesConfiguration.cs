using System.Collections.Generic;
using PokemonCardTraderBot.Common.Attributes;
using PokemonCardTraderBot.Common.Enums;

namespace PokemonCardTraderBot.Common.Configurations
{
    [ConfigurationFileName("card_rarities_config")]
    public class RaritiesConfiguration : Dictionary<RarityType, RarityInfo>, IDefaultConfiguration
    {
    }

    public class RarityInfo
    {
        public List<string> Rarities { get; set; }
        public double DropChance { get; set; }
    }
}