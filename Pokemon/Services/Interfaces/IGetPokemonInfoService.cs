using Pokemon.Models;

namespace Pokemon.Services.Interfaces;

public interface IGetPokemonInfoService
{
    Task<PokemonInfo> GetPokemonInfo(string name);
}