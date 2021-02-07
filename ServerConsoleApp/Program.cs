using System;
using System.Threading;
using System.Threading.Tasks;
using CodeService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ServerConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            Startup.Init();

            var codeService = Startup.ServiceProvider.GetService<ICodeService>();

            if (codeService is null)
            {
                Console.WriteLine("Failed to launch the service. IoC container could not resolve it.");
                return;
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(() => codeService.Start(cancellationTokenSource.Token), cancellationTokenSource.Token);

            Console.WriteLine("Press enter to stop the service...");
            Console.ReadLine();

            cancellationTokenSource.Cancel();

            task.Wait();
            Console.WriteLine("Service was stopped successfully.");
            Console.WriteLine();

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
