using System.Text.Json;
using Pokemon.Models;
using Pokemon.Services.Interfaces;

namespace Pokemon.Services;

public class GetPokemonTypeService : IGetPokemonTypeService
{
    private HttpClient _httpClient;

    public GetPokemonTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PokemonType>> GetPokemonType(PokemonInfo pokemonInfo)
    {
        
            var pokemonInfoTypes = new List<PokemonType>();

            foreach (var types in pokemonInfo.types)
            {

                var response =
                    await _httpClient.GetAsync(new Uri(types.type.url));

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var pokemonTypeInfo = JsonSerializer.Deserialize<PokemonType>(jsonResponse);
                    pokemonInfoTypes.Add(pokemonTypeInfo);
                }
                else
                {
                    pokemonInfoTypes.Add(new PokemonType(){ClientErrorMessages = [$"There was a problem calling pokeAPI for damage type {types.type.name}"] });
                }
            }

            return pokemonInfoTypes;
       
            
    }
    
}