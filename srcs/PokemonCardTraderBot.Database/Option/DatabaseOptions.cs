namespace PokemonCardTraderBot.Database.Option
{
    public class DatabaseOptions
    {
        public const string Name = nameof(DatabaseOptions);
        public string Ip { get; }
        public string Username { get; }
        public string Password { get; }
        public string Database { get; }
        public ushort Port { get; }

        public override string ToString() => $"Host={Ip};Port={Port.ToString()};Database={Database};Username={Username};Password={Password}";
    }
}