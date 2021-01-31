using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CodeService.Enums;
using CodeService.Interfaces;
using CodeService.Models;
using Microsoft.Extensions.Logging;

namespace CodeService
{
    public class CodeService : ICodeService
    {
        private readonly ILogger<ICodeService> _logger;
        private readonly IDataEncoder _dataEncoder;

        private readonly ICodesCollection _codes;

        public CodeService(IDataEncoder dataEncoder, ICodesCollection codesCollection, ILogger<ICodeService> logger)
        {
            _dataEncoder = dataEncoder;
            _logger = logger;
            _codes = codesCollection;
        }

        public void Start()
        {
            var tasks = new Task[2];

            tasks[0] = Task.Run(StartGeneratorService);
            tasks[1] = Task.Run(StartUseCodeService);

            Task.WaitAll(tasks);
        }

        public Task StartGeneratorService()
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 30_000);
            listener.Start();

            while (true)
            {
                _logger.LogInformation("Generator is listening for connection requests...");
                
                using var client = listener.AcceptTcpClient();
                _logger.LogInformation("Client connected...");

                using var stream = client.GetStream();

                _logger.LogInformation("Unpacking request data...");
                var requestData = _dataEncoder.DecodeGenerateRequest(stream);

                _logger.LogInformation("Generating codes...");
                var success = requestData.NumberOfCodesToGenerate > 0 
                              && _codes.GenerateNewCodes(requestData.NumberOfCodesToGenerate, requestData.CodeLength);

                _logger.LogInformation($"Generation was {(success ? "successful" : "failed")}...");
                _logger.LogInformation(string.Join('\n', _codes.Select(c => c.Value)));

                var responseAsByteArray = _dataEncoder.EncodeGenerateResponse(success);

                _logger.LogInformation("Sending data back...");
                stream.Write(responseAsByteArray, 0, 1);

                _logger.LogInformation("Connection concluded...\n\n");
            }
        }

        public Task StartUseCodeService()
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 30_001);
            listener.Start();

            while (true)
            {
                _logger.LogInformation("Code checker is listening for connection requests...");
                
                using var client = listener.AcceptTcpClient();
                _logger.LogInformation("Client connected...");

                using var stream = client.GetStream();

                _logger.LogInformation("Unpacking request data...");
                var codeToLookUp = _dataEncoder.DecodeUseCodeRequest(stream);

                _logger.LogInformation($"Checking code {codeToLookUp.Value}...");
                var response = _codes.CheckCode(codeToLookUp);

                _logger.LogInformation($"The code returned state {response}...");

                var responseAsByteArray = _dataEncoder.EncodeUseCodeResponse(response);

                _logger.LogInformation("Sending data back...");
                stream.Write(responseAsByteArray, 0, 1);

                _logger.LogInformation("Connection concluded...\n\n");
            }
        }
    }
}
