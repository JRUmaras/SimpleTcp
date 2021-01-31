using System;
using System.Collections.Generic;
using System.Text;
using CodeService.Helpers;
using CodeService.Models;
using CodeService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

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
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                })
                .AddScoped<ICodeService, CodeService.CodeService>()
                .AddScoped<ICodesCollection, CodesCollection>()
                .BuildServiceProvider();
        }
    }
}
