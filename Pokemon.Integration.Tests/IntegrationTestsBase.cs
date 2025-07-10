using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Pokemon.Services;
using Pokemon.Services.Interfaces;

namespace Pokemon.Integration.Tests
{
    public class IntegrationTestsBase
    {
        public IGetPokemonInfoService GetPokemonInfoService { get; private set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddScoped<IGetPokemonInfoService, GetPokemonInfoService>();
            
            var serviceProvider = services.BuildServiceProvider();

            GetPokemonInfoService = serviceProvider.GetRequiredService<IGetPokemonInfoService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
           //put anything that needs to be disposed of after use here like db integration tests
        }
    }
}
