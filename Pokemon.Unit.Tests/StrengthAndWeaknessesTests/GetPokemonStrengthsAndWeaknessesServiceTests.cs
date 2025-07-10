using Moq;
using Pokemon.Models;
using Pokemon.Services;
using Pokemon.Services.Interfaces;
using System.Xml.Linq;
using Xunit;

namespace Pokemon.Unit.Tests.StrengthAndWeaknessesTests;

public class GetPokemonStrengthsAndWeaknessesServiceTests
{
    [Fact]
    public async Task Should_Display_Error_When_404_Returned_From_Pokemon_API()
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var mockGetPokemonTypeService = new Mock<IGetPokemonTypeService>();
        mockGetPokemonTypeService.Setup(m => m.GetPokemonType(It.IsAny<PokemonInfo>()))
            .ReturnsAsync(new List<PokemonType>());

        var mockGetPokemonInfoService = new Mock<IGetPokemonInfoService>();
        mockGetPokemonInfoService.Setup(m => m.GetPokemonInfo(It.IsAny<string>()))
            .ReturnsAsync(new PokemonInfo() { ClientErrorMessages = ["Error"] });

        var mainService = new GetPokemonStrengthsAndWeaknessesService(
            mockGetPokemonTypeService.Object,
            mockGetPokemonInfoService.Object
        );

        await mainService.GetPokemonStrengthsAndWeaknesses("test");
        var outPut = stringWriter.ToString();

        Assert.Equal("Error\r\n", outPut);

    }

    [Fact]
    public async Task Should_Display_Error_When_404_Returned_From_PokemonType_API()
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var mockGetPokemonTypeService = new Mock<IGetPokemonTypeService>();
        mockGetPokemonTypeService.Setup(m => m.GetPokemonType(It.IsAny<PokemonInfo>()))
            .ReturnsAsync(new List<PokemonType> { new() { ClientErrorMessages = ["Error"] } });

        var mockGetPokemonInfoService = new Mock<IGetPokemonInfoService>();
        mockGetPokemonInfoService.Setup(m => m.GetPokemonInfo(It.IsAny<string>()))
            .ReturnsAsync(new PokemonInfo() { types = [new()] });

        var mainService = new GetPokemonStrengthsAndWeaknessesService(
            mockGetPokemonTypeService.Object,
            mockGetPokemonInfoService.Object
        );

        await mainService.GetPokemonStrengthsAndWeaknesses("test");
        var outPut = stringWriter.ToString();

        Assert.Equal("Error\r\n", outPut);

    }

    [Fact]
    public async Task Should_Display_Error_When_No_Types_Returned_From_PokemonType_API()
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var mockGetPokemonTypeService = new Mock<IGetPokemonTypeService>();
        mockGetPokemonTypeService.Setup(m => m.GetPokemonType(It.IsAny<PokemonInfo>()))
            .ReturnsAsync(new List<PokemonType> { new() { ClientErrorMessages = ["Error"] } });

        var mockGetPokemonInfoService = new Mock<IGetPokemonInfoService>();
        mockGetPokemonInfoService.Setup(m => m.GetPokemonInfo(It.IsAny<string>()))
            .ReturnsAsync(new PokemonInfo());

        var mainService = new GetPokemonStrengthsAndWeaknessesService(
            mockGetPokemonTypeService.Object,
            mockGetPokemonInfoService.Object
        );

        await mainService.GetPokemonStrengthsAndWeaknesses("test");
        var outPut = stringWriter.ToString();

        Assert.Equal($"no damage types for pokemon test were returned from pokeAPI. please revise your search and try again\r\n", outPut);

    }

    [Fact]
    public async Task Should_Display_Error_When_Id_Entered()
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var mockGetPokemonTypeService = new Mock<IGetPokemonTypeService>();
        var mockGetPokemonInfoService = new Mock<IGetPokemonInfoService>();


        var mainService = new GetPokemonStrengthsAndWeaknessesService(
            mockGetPokemonTypeService.Object,
            mockGetPokemonInfoService.Object
        );

        await mainService.GetPokemonStrengthsAndWeaknesses("1");
        var outPut = stringWriter.ToString();

        Assert.Equal("value 1 contains invalid characters, please ensure the value is a pokemon name only\r\n", outPut);

    }

    [Theory]
    [InlineData("flying","ground","earth", "wind","fire","lightening","water","air")]
    public async Task Should_Return_TypeInfo_Successfully_With_One_Type_Showing_ClientError(string damageType1, string damageType2,string doubleDamageTo, string halfDamageFrom, string noDamageFrom, string doubleDamageFrom, string halfDamageTo, string noDamageTo)
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var mockGetPokemonTypeService = new Mock<IGetPokemonTypeService>();
        mockGetPokemonTypeService.Setup(m => m.GetPokemonType(It.IsAny<PokemonInfo>()))
            .ReturnsAsync(new List<PokemonType> { new() { name = damageType2, ClientErrorMessages = ["Error"] },
                new() {
                    name = damageType2,
                    damage_relations =
                    new(){
                        double_damage_to = new()
                        {
                            new()
                            {
                                name=doubleDamageTo
                            }
                        },
                        half_damage_from = new()
                        {
                            new()
                            {
                                name = halfDamageFrom
                            }
                        },
                        no_damage_from = new()
                        {
                            new()
                            {
                                name = noDamageFrom
                            }
                        },
                        no_damage_to = new()
                        {
                            new()
                            {
                                name = noDamageTo
                            }
                        },
                        half_damage_to = new()
                        {
                            new()
                            {
                                name = halfDamageTo
                            }
                        },
                        double_damage_from = new()
                        {
                            new()
                            {
                                name = doubleDamageFrom
                            }
                        }
                    }
                }
            });

        var mockGetPokemonInfoService = new Mock<IGetPokemonInfoService>();
        mockGetPokemonInfoService.Setup(m => m.GetPokemonInfo(It.IsAny<string>()))
            .ReturnsAsync(new PokemonInfo() { types = new() { new() } });

        var mainService = new GetPokemonStrengthsAndWeaknessesService(
            mockGetPokemonTypeService.Object,
            mockGetPokemonInfoService.Object
        );

        await mainService.GetPokemonStrengthsAndWeaknesses("test");
        var outPut = stringWriter.ToString();

        Assert.Equal($"Error\r\ntest with type {damageType2} is strong against:\r\n-{doubleDamageTo}\r\n-{halfDamageFrom}\r\n-{noDamageFrom}\r\ntest with type {damageType2} does double damage to:\r\n-{doubleDamageTo}\r\ntest with type {damageType2} takes half damage from:\r\n-{halfDamageFrom}\r\ntest with type {damageType2} takes no damage from:\r\n-{noDamageFrom}\r\ntest with type {damageType2} is weak against:\r\n-{doubleDamageFrom}\r\n-{halfDamageTo}\r\n-{noDamageTo}\r\ntest with type {damageType2} takes double damage from:\r\n-{doubleDamageFrom}\r\ntest with type {damageType2} does half damage to:\r\n-{halfDamageTo}\r\ntest with type {damageType2} does no damage to:\r\n-{noDamageTo}\r\n", outPut);
    }

}

