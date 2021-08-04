using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCardTraderBot.Common.Configurations;
using PokemonCardTraderBot.Common.Enums;
using PokemonCardTraderBot.Common.Services;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Common.Extensions
{
    public static class CardExtensions
    {
        public static List<PokemonCard> ToBooster(this List<PokemonCard> allCards, IRandomService randomService,
            RaritiesConfiguration configuration)
        {
            List<PokemonCard> boosterCards = new();
            boosterCards.AddRange(allCards.SelectCommonCards().PickRandomCards(randomService, 5));
            boosterCards.AddRange(allCards.SelectUncommonCards().PickRandomCards(randomService, 3));
            boosterCards.AddRange(allCards.SelectHoloCards(configuration).PickRandomCards(randomService, 1));
            boosterCards.AddRange(allCards.SelectRarePlusCards(randomService, configuration).PickRandomCards(randomService, 1));
            return boosterCards;
        }

        public static List<PokemonCard> PickRandomCards(this List<PokemonCard> cards, IRandomService randomService, int amount)
        {
            if (!cards.Any())
            {
                return null;
            }
            
            List<PokemonCard> pickedCards = new();
            for (int i = 0; i < amount; i++)
            {
                pickedCards.Add(cards[randomService.NextInt(cards.Count)]);
            }
            return pickedCards;
        }

        public static List<PokemonCard> SelectCommonCards(this List<PokemonCard> cards)
            => cards.FindAll(x => x.Rarity == "Common");
        
        public static List<PokemonCard> SelectUncommonCards(this List<PokemonCard> cards)
            => cards.FindAll(x => x.Rarity == "Uncommon");

        public static List<PokemonCard> SelectHoloCards(this List<PokemonCard> cards,
            RaritiesConfiguration configuration)
        {
            return cards.FindAll(x => (x.Rarity.Contains("Holo") || x.Rarity.Contains("Shining"))
                                                                 && !configuration[RarityType.UltraRare].Rarities.Contains(x.Rarity)
                                                                 && !configuration[RarityType.SecretRare].Rarities
                                                                     .Contains(x.Rarity));
        }
        
        public static List<PokemonCard> SelectRarePlusCards(this List<PokemonCard> cards, IRandomService randomService,
            RaritiesConfiguration configuration)
        {
            int rdmNumber = randomService.NextInt(0, 1000);
            Console.WriteLine(rdmNumber.ToString());

            if (rdmNumber < configuration[RarityType.SecretRare].DropChance * 1000)
            {
                return cards.FindAll(x => configuration[RarityType.SecretRare].Rarities.Contains(x.Rarity));
            }

            if (rdmNumber < configuration[RarityType.UltraRare].DropChance * 1000)
            {
                return cards.FindAll(x => configuration[RarityType.UltraRare].Rarities.Contains(x.Rarity));
            }

            return cards.FindAll(x => configuration[RarityType.Rare].Rarities.Contains(x.Rarity));
        }
    }
}