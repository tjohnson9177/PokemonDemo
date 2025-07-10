namespace Pokemon.Models;

public class PokemonInfo
{
    public List<types> types { get; set; }
    public List<string> ClientErrorMessages { get; set; }
}

public class types
{
    public int slot { get; set; }
    public type type { get; set; }
}
public class type
{
    public string name { get; set; }
    public string url { get; set; }
}