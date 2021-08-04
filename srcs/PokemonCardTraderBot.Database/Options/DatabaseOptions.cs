namespace PokemonCardTraderBot.Database.Options
{
    public class DatabaseOptions
    {
        public const string Name = nameof(DatabaseOptions);
        public string Ip { get; set; }
        public string Username { get; set; }
        public string Password { get; set;  }
        public string Database { get; set; }
        public ushort Port { get; set;  }

        public override string ToString() => $"Host={Ip};Port={Port.ToString()};Database={Database};Username={Username};Password={Password}";
    }
}