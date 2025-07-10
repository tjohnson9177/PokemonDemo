using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Pokemon.Models;
using Pokemon.Services;

namespace Pokemon.Integration.Tests.StrengthsAndWeaknessesTests
{
    public class GetPokemonInfoServiceIntegrationTests : IntegrationTestsBase
    {
        [Test]
        public async Task Should_Return_Success_With_Valid_Name()
        {
            var result = await GetPokemonInfoService.GetPokemonInfo("charizard");
            Assert.That(result.types.Any());
            Assert.That(result.types.Select(x=>x.type.name).Any());
        }
    }
}
