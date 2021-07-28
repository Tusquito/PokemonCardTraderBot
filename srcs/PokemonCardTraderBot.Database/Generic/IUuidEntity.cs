using System;

namespace PokemonCardTraderBot.Database.Generic
{
    public interface IUuidEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}