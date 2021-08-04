using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokemonCardTraderBot.Common.Generic;
using PokemonCardTraderBot.Database.Context;
using PokemonCardTraderBot.Database.Generic;
using PokemonCardTraderBot.Database.Options;
using PokemonCardTraderBot.Database.Utils;

namespace PokemonCardTraderBot.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Name));
            services.AddDbContext<BotContext>((provider, builder)  =>
            {
                string dbConfig = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value.ToString();
                builder
                    .UseNpgsql(dbConfig)
                    .ConfigureWarnings(s => s.Log(
                        (RelationalEventId.CommandExecuting, LogLevel.Debug),
                        (RelationalEventId.CommandExecuted, LogLevel.Debug)
                    ));
            }, ServiceLifetime.Transient, ServiceLifetime.Singleton);
            

            services.AddDbContextFactory<BotContext>((provider, builder) =>
            {
                string dbConfig = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value.ToString();
                builder
                    .UseNpgsql(dbConfig)
                    .ConfigureWarnings(s => s.Log(
                        (RelationalEventId.CommandExecuting, LogLevel.Debug),
                        (RelationalEventId.CommandExecuted, LogLevel.Debug)
                    ));
            });
        }
        
        public static void TryAddMappedAsyncUuidRepository<TEntity, TDto>(this IServiceCollection services)
            where TEntity : class, IUuidEntity
            where TDto : class, IUuidDto
        {
            services.AddTransient<IGenericMapper<TEntity, TDto>, CustomMapsterMapper<TEntity, TDto>>();
            services.AddTransient<IGenericAsyncUuidRepository<TDto>, GenericMappedAsyncUuidRepository<TEntity, TDto>>();
        }
    }
}