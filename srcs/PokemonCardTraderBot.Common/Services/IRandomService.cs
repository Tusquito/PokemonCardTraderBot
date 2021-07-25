namespace PokemonCardTraderBot.Common.Services
{
    public interface IRandomService
    {
        public int NextInt();
        public int NextInt(int max);
        public int NextInt(int min, int max);
    }
}