using System;
using PokemonCardTraderBot.Common.Services;

namespace PokemonCardTraderBot.Core.Services
{
    public class RandomService : IRandomService
    {
        private static readonly Random Random = new(Guid.NewGuid().GetHashCode());
        public int NextInt() => Random.Next();
        public int NextInt(int max) => Random.Next(max);
        public int NextInt(int min, int max) => Random.Next(min, max);
    }
}