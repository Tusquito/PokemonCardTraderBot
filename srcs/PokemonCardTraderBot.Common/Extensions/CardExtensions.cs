using System.Collections.Generic;
using System.Diagnostics;
using PokemonCardTraderBot.Common.Services;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Common.Extensions
{
    public static class CardExtensions
    {
        public static List<PokemonCard> ToBooster(this List<PokemonCard> allCards, IRandomService randomService)
        {
            List<PokemonCard> boosterCards = new();
            boosterCards.AddRange(allCards.SelectCommonCards().PickRandomCards(randomService, 5));
            boosterCards.AddRange(allCards.SelectUncommonCards().PickRandomCards(randomService, 3));
            boosterCards.AddRange(allCards.SelectHoloCards().PickRandomCards(randomService, 1));
            boosterCards.AddRange(allCards.SelectRareCards().PickRandomCards(randomService, 1));
            return boosterCards;
        }

        public static List<PokemonCard> PickRandomCards(this List<PokemonCard> cards, IRandomService randomService, int amount)
        {
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
        
        public static List<PokemonCard> SelectHoloCards(this List<PokemonCard> cards)
            => cards.FindAll(x => (x.Rarity.Contains("Holo") || x.Rarity.Contains("Shining"))
                                  && !x.Rarity.Contains("GX")
                                  && !x.Rarity.Contains("EX")
                                  && !x.Rarity.Contains("VMAX"));
        
        public static List<PokemonCard> SelectRareCards(this List<PokemonCard> cards)
            => cards.FindAll(x => x.Rarity.Contains("Rare") || x.Rarity.Contains("LEGEND"));
    }
}