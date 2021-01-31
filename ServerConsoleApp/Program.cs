using CodeService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
