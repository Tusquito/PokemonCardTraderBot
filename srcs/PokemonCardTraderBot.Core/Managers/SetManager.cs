using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PokemonCardTraderBot.Common.Attributes;
using PokemonCardTraderBot.Common.Configurations;
using PokemonCardTraderBot.Common.Options;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Core.Managers
{
    public class SetManager
    {
        private const string CacheKey = "sets-cache-key";

        private readonly IMemoryCache _cache;
        private readonly CardSetsConfiguration _cardSetsConfiguration;
        private readonly ConfigurationFilesOptions _configurationFilesOptions;

        public SetManager(IMemoryCache cache, CardSetsConfiguration cardSetsConfiguration, IOptions<ConfigurationFilesOptions> configurationFilesOptions)
        {
            _cache = cache;
            _cardSetsConfiguration = cardSetsConfiguration;
            _configurationFilesOptions = configurationFilesOptions.Value;
        }

        public async Task<List<SetData>> GetAllAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<SetData> sets))
            {
                return sets;
            }

            sets = await RefreshCardsAsync();

            return sets;
        }

        public async Task<List<SetData>> RefreshCardsAsync()
        {
            var sets = (await Sets.AllAsync())
                .Where(x => !_cardSetsConfiguration.ContainsKey(x.Code) || !_cardSetsConfiguration[x.Code].IsBlacklisted)
                .ToList();
            
            _cache.Set(CacheKey, sets, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(7)));
            await SaveConfigurationFileAsync();
            return sets;
        }

        public async Task AddOrUpdateConfigurationEntry(CardSetInfo setInfo)
        {
            _cardSetsConfiguration[setInfo.Code] = setInfo;
            await RefreshCardsAsync();
        }

        public async Task<SetData> GetBySetCodeAsync(string setCode)
        {
            return (await GetAllAsync()).Find(x => x.Code == setCode);
        }
        
        public async Task<SetData> SearchAsync(string search)
        {
            return (await GetAllAsync()).Find(x => x.Name.ToLower().Contains(search.ToLower()))
                ?? (await GetAllAsync()).Find(x => x.Code.ToLower().Contains(search.ToLower()));
        }

        public async Task SaveConfigurationFileAsync()
        {
            var json = JsonSerializer.Serialize(_cardSetsConfiguration, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(
                $"{_configurationFilesOptions.FolderPath}/{typeof(CardSetsConfiguration).GetCustomAttribute<ConfigurationFileNameAttribute>()?.Name}.json",
                json);
        }

        
    }
}