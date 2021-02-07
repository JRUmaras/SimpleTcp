using System;
using System.Net.Sockets;

namespace ClientConsoleApp
{
    internal class Program
    {
        private const int _bufferSizeGenerateOp = 3;
        private const string _ip = "127.0.0.1";
        private const int _portForGeneratingCodes = 30_000;
        private const int _portForUsingCode = 30_001;

        private static void Main()
        {
            var exit = false;

            while (!exit)
            {
                Console.WriteLine("What do you want to do? Generate codes - press G, check code - press C, exit - E");
                var command = Console.ReadLine().ToUpper();

                try
                {
                    switch (command)
                    {
                        case "G":
                            Console.WriteLine("How many codes to insert?");
                            var numberOfCodesToGenerate = ushort.Parse(Console.ReadLine());
                            Console.WriteLine("How long should each code be?");
                            var codeLength = int.Parse(Console.ReadLine());
                            GenerateRequest(numberOfCodesToGenerate, codeLength);
                            break;
                        case "C":
                            Console.WriteLine("Enter code you want to lookup:");
                            var codeValue = Console.ReadLine();
                            UseCodeRequest(codeValue);
                            break;
                        case "E":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid command, try again.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Operation failed due to an exception!\n" + e.Message);
                }

                Console.WriteLine();
            }
        }

        private static void GenerateRequest(ushort numberOfCodesToGenerate, int codeLength)
        {
            
                using var client = new TcpClient(_ip, _portForGeneratingCodes);

                Console.WriteLine(
                    $"Preparing input, generating {numberOfCodesToGenerate} codes of length {codeLength}...");
                var input = ClientDataEncoder.EncodeGenerateRequest(numberOfCodesToGenerate, codeLength);

                Console.WriteLine("Sending request...");
                using var stream = client.GetStream();
                stream.Write(input, 0, _bufferSizeGenerateOp);

                Console.WriteLine("Reading response...");
                var response = ClientDataEncoder.DecodeGenerateResponse(stream);
                Console.WriteLine($"Codes were generated {(response ? "successfully" : "unsuccessfully")}...");
                Console.WriteLine("See you later, alligator!\n");
        }

        private static void UseCodeRequest(string codeValue)
        {
            using var client = new TcpClient(_ip, _portForUsingCode);

            Console.WriteLine("Preparing input...");
            var input = ClientDataEncoder.EncodeUseCodeRequest(codeValue);

            Console.WriteLine("Sending request...");
            using var stream = client.GetStream();
            stream.Write(input, 0, input.Length);

            Console.WriteLine("Reading response...");
            var response = ClientDataEncoder.DecodeUseCodeResponse(stream);
            Console.WriteLine($"Code state is {response}...");
            Console.WriteLine("See you later, alligator!");
        }
    }
}
