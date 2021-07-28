using System;
using PokemonCardTraderBot.Common.Generic;

namespace PokemonCardTraderBot.Database.Generic
{
    public interface IGenericAsyncUuidRepository<TDto> : IGenericAsyncRepository<TDto, Guid> 
        where TDto : class, IUuidDto
    { }
}