using Pokemon.Models;

namespace Pokemon.Services.Interfaces;

public interface IGetPokemonTypeService
{
    Task<List<PokemonType>> GetPokemonType(PokemonInfo pokemonInfo);
}