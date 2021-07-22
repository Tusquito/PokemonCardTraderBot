using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Core.Managers
{
    public class CardsManager
    {
        private const string CacheKey = "{0}-set-cards-cache-key";
        
        private readonly IMemoryCache _cache;

        public CardsManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<List<PokemonCard>> GetOrAddBySetCode(string setCode)
        {
            if (_cache.TryGetValue(string.Format(CacheKey, setCode), out List<PokemonCard> cards))
            {
                return cards;
            }
        
            cards = await Card.AllAsync(new Dictionary<string, string>
            {
                {"setCode", setCode}
            });
            
            if (cards == null)
            {
                return null;
            }
            
            _cache.Set(string.Format(CacheKey, setCode), cards, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(7)));

            return cards;
        }
    }
}