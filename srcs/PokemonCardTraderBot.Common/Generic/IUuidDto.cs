using System;

namespace PokemonCardTraderBot.Common.Generic
{
    public interface IUuidDto : IDto
    {
        public Guid Id { get; set; }
    }
}