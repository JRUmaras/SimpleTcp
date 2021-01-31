using System;
using CodeService.Helpers;
using CodeService.Models;
using CodeService.Interfaces;
using CodeService.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ServerConsoleApp
{
    internal static class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Init()
        {
            ServiceProvider = new ServiceCollection()
                .AddScoped<IDataEncoder, DataEncoder>()
                .AddScoped<ICodeGenerator, CodeGenerator>()
                .AddScoped<ICodeService, CodeService.CodeService>()
                .AddScoped<ICodesCollection, CodesCollection>()
                .AddSingleton<IRepository, JsonRepository>()
                .BuildServiceProvider();
        }
    }
}
