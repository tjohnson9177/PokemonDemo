using System.Text.Json;
using Pokemon.Models;
using Pokemon.Services.Interfaces;

namespace Pokemon.Services;

public class GetPokemonInfoService : IGetPokemonInfoService
{
    private readonly HttpClient _httpClient;
    public GetPokemonInfoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PokemonInfo> GetPokemonInfo(string name)
    {
        var response = await _httpClient.GetAsync(new Uri($"https://pokeapi.co/api/v2/pokemon/{name}"));

        if (!response.IsSuccessStatusCode)
        {
            return new PokemonInfo(){ClientErrorMessages = [$"Pokemon with name {name} was not found in pokeAPI, please revise your search"] };
        }
        
        var responsejson = await response.Content.ReadAsStringAsync();
        
        var pokemonInfo = JsonSerializer.Deserialize<PokemonInfo>(responsejson);

        return pokemonInfo;
    }
}