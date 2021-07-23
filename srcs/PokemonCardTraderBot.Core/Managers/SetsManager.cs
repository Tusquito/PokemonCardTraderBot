using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Core.Managers
{
    public class SetsManager
    {
        private const string CacheKey = "sets-cache-key";

        private readonly IMemoryCache _cache;

        public SetsManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<List<SetData>> GetOrAddAllAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<SetData> sets))
            {
                return sets;
            }
            
            sets = (await Sets.AllAsync())
                .Where(x => !x.Series.Contains("Other")
                            && !x.Series.Contains("POP")
                            && !x.Series.Contains("NP")
                            && !x.Series.Contains("E-Card")
                            && !x.Series.Contains("Neo")
                            && !x.Series.Contains("Gym"))
                .ToList();
            
            _cache.Set(CacheKey, sets, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return sets;
        }

        public async Task<SetData> GetBySetCodeAsync(string setCode)
        {
            return (await GetOrAddAllAsync()).Find(x => x.Code == setCode);
        }
        
        public async Task<SetData> GetByNameAsync(string name)
        {
            return (await GetOrAddAllAsync()).Find(x => x.Name.ToLower().Contains(name.ToLower()));
        }
    }
}