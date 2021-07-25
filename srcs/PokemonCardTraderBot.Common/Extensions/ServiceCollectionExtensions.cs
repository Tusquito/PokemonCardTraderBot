using System.IO;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PokemonCardTraderBot.Common.Attributes;
using PokemonCardTraderBot.Common.Configurations;

namespace PokemonCardTraderBot.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigurationFile<T>(this IServiceCollection services)
            where T : class, IDefaultConfiguration
        {
            var fileName =  typeof(T).GetCustomAttribute<ConfigurationNameAttribute>()?.Name;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var temp = JsonSerializer.Deserialize<T>(File.ReadAllText($"../../config/{fileName}.json"));
            if (temp == null)
            {
                return;
            }
            services.AddSingleton(temp);
        }

        public static void AddConfigurationFiles(this IServiceCollection services)
        {
            services.AddConfigurationFile<RaritiesConfiguration>();
        }
    }
}