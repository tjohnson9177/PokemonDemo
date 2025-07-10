using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pokemon.Services;
using Pokemon.Services.Interfaces;

namespace Pokemon
{
    internal class Program
    {

        static async Task Main(string[] args)
        {

            bool continueRunning = true;
            var host = CreateHostBuilder(args).Build();
            var service = host.Services.GetRequiredService<IGetPokemonStrengthsAndWeaknessesService>();
            while (continueRunning)
            {
                Console.WriteLine("Please Enter Pokemon Name:");
                string name = Console.ReadLine();
                await service.GetPokemonStrengthsAndWeaknesses(name);
                
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {

                    services.AddHttpClient();
                    services.AddSingleton<IGetPokemonTypeService,GetPokemonTypeService>();
                    services.AddSingleton<IGetPokemonInfoService,GetPokemonInfoService>();
                    services.AddSingleton<IGetPokemonStrengthsAndWeaknessesService,GetPokemonStrengthsAndWeaknessesService>();

                });
    }


}
