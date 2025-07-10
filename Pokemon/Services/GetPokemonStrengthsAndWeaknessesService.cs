using Pokemon.Models;
using Pokemon.Services.Interfaces;

namespace Pokemon.Services;

public class GetPokemonStrengthsAndWeaknessesService : IGetPokemonStrengthsAndWeaknessesService
{
    private readonly IGetPokemonTypeService _getPokemonTypeService;
    private readonly IGetPokemonInfoService _getPokemonInfoService;

    public GetPokemonStrengthsAndWeaknessesService(
         IGetPokemonTypeService getPokemonTypeService
        ,IGetPokemonInfoService getPokemonInfoService)
    {
        _getPokemonTypeService = getPokemonTypeService;
        _getPokemonInfoService = getPokemonInfoService;
    }

    public async Task GetPokemonStrengthsAndWeaknesses(string name)
    {
        //PokeAPI accapts name OR id, but for ease of use i am restricting the input to only pokemon names
        if (name.All(char.IsNumber))
        {
            Console.WriteLine($"value {name} contains invalid characters, please ensure the value is a pokemon name only");
            return;
        }
        
        var pokemonInfo = await _getPokemonInfoService.GetPokemonInfo(name);

        if (pokemonInfo.ClientErrorMessages?.Count()>0)
        {
            pokemonInfo.ClientErrorMessages.ForEach(x=>Console.WriteLine(x));
            return;
        }

        if (pokemonInfo.types?.Any()!=true)
        {
            Console.WriteLine($"no damage types for pokemon {name} were returned from pokeAPI. please revise your search and try again");
            return;
        }
        var typeInfo = await _getPokemonTypeService.GetPokemonType(pokemonInfo);

        foreach (var type in typeInfo)
        {

            if (type.ClientErrorMessages?.Count>0)
            {
                type.ClientErrorMessages.ForEach(x=>Console.WriteLine(x));
                continue;
            }

            var doubleDamageTo = type.damage_relations.double_damage_to.Select(x => x.name);
            var halfDamageFrom = type.damage_relations.half_damage_from.Select(x => x.name);
            var noDamageFrom = type.damage_relations.no_damage_from.Select(x => x.name);

            var strongAgainst = new List<string>();
            strongAgainst.AddRange(doubleDamageTo);
            strongAgainst.AddRange(halfDamageFrom);
            strongAgainst.AddRange(noDamageFrom);

            Console.WriteLine($"{name} with type {type.name} is strong against:");
            strongAgainst.ForEach(x => Console.WriteLine($"-{x}"));

            if (doubleDamageTo.Any())
            {
                Console.WriteLine($"{name} with type {type.name} does double damage to:");
                doubleDamageTo.ToList().ForEach(x =>
                {
                    Console.WriteLine($"-{x}");
                });
            }

            
            if (halfDamageFrom.Any())
            {
                Console.WriteLine($"{name} with type {type.name} takes half damage from:");
                halfDamageFrom.ToList().ForEach(x =>
                {
                    Console.WriteLine($"-{x}");

                });
            }

            if (noDamageFrom.Any())
            {
                Console.WriteLine($"{name} with type {type.name} takes no damage from:");
                noDamageFrom.ToList().ForEach(x =>
                {
                    Console.WriteLine($"-{x}");
                });
            }


            var doubleDamageFrom = type.damage_relations.double_damage_from.Select(x => x.name);
            var halfDamageTo = type.damage_relations.half_damage_to.Select(x => x.name);
            var noDamageTo = type.damage_relations.no_damage_to.Select(x => x.name);

            var weakAgainst = new List<string>();
            weakAgainst.AddRange(doubleDamageFrom);
            weakAgainst.AddRange(halfDamageTo);
            weakAgainst.AddRange(noDamageTo);

            Console.WriteLine($"{name} with type {type.name} is weak against:");
            weakAgainst.ForEach(x => Console.WriteLine($"-{x}"));

            if (doubleDamageFrom.Any())
            {
                Console.WriteLine($"{name} with type {type.name} takes double damage from:");
                doubleDamageFrom.ToList().ForEach(x =>
                {

                    Console.WriteLine($"-{x}");

                });
            }

            
            if (halfDamageTo.Any())
            {
                Console.WriteLine($"{name} with type {type.name} does half damage to:");

                halfDamageTo.ToList().ForEach(x =>
                {
                    Console.WriteLine($"-{x}");
                });

            }

            
            if (noDamageTo.Any())
            {
                Console.WriteLine($"{name} with type {type.name} does no damage to:");

                noDamageTo.ToList().ForEach(x =>
                {
                    Console.WriteLine($"-{x}");
                });

            }

        }

    }
}