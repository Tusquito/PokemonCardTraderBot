using System.IO;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokemonCardTraderBot.Common.Attributes;
using PokemonCardTraderBot.Common.Configurations;
using PokemonCardTraderBot.Common.Options;

namespace PokemonCardTraderBot.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigurationFile<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, IDefaultConfiguration
        {
            var fileName =  typeof(T).GetCustomAttribute<ConfigurationFileNameAttribute>()?.Name;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }
            
            var filesConfig = configuration.GetSection(ConfigurationFilesOptions.Name).Get<ConfigurationFilesOptions>();
            
            if (string.IsNullOrWhiteSpace(filesConfig.FolderPath))
            {
                return;
            }
            
            var temp = JsonSerializer.Deserialize<T>(File.ReadAllText($"{filesConfig.FolderPath}/{fileName}.json"));
            if (temp == null)
            {
                return;
            }
            services.AddSingleton(temp);
        }

        public static void AddConfigurationFiles(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfigurationFilesOptions>(configuration.GetSection(ConfigurationFilesOptions.Name));
            services.AddConfigurationFile<RaritiesConfiguration>(configuration);
            services.AddConfigurationFile<CardSetsConfiguration>(configuration);
        }
    }
}