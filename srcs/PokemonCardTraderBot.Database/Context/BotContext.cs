using Microsoft.EntityFrameworkCore;

namespace PokemonCardTraderBot.Database.Context
{
    public class BotContext : DbContext
    {
        public BotContext(DbContextOptions<BotContext> options) : base(options) { }
    }
}