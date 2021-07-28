using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using PokemonCardTraderBot.Database.Option;

namespace PokemonCardTraderBot.Database.Context
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BotContext>
    {
        private readonly DatabaseOptions _options;
        public DesignTimeContextFactory(IOptions<DatabaseOptions> options)
        {
            _options = options.Value;
        }
        public BotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BotContext>();
            optionsBuilder.UseNpgsql(_options.ToString());
            return new BotContext(optionsBuilder.Options);
        }
    }
}