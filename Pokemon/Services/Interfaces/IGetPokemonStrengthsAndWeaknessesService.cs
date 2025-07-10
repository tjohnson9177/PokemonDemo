namespace Pokemon.Services.Interfaces;

public interface IGetPokemonStrengthsAndWeaknessesService
{
    Task GetPokemonStrengthsAndWeaknesses(string name);
}