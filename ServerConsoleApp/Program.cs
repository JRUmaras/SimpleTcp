using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CodeService;
using CodeService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup.Init();

            var codeService = Startup.ServiceProvider.GetService<ICodeService>();

            codeService.Start();
        }
    }
}
