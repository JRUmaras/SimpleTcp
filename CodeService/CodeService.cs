using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CodeService.Interfaces;

namespace CodeService
{
    public class CodeService : ICodeService
    {
        private readonly IDataEncoder _dataEncoder;
        private readonly ICodesCollection _codes;
        private readonly IRepository _repository;

        public CodeService(IDataEncoder dataEncoder, ICodesCollection codesCollection, IRepository repository)
        {
            _dataEncoder = dataEncoder;
            _codes = codesCollection;
            _repository = repository;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            _codes.AddRange(await Task.Run(_repository.LoadCodes, cancellationToken));

            var tasks = new Task[2];

            tasks[0] = Task.Run(() => StartBaseService(IPAddress.Parse("127.0.0.1"), 30_000, GenerateService, cancellationToken), cancellationToken);
            tasks[1] = Task.Run(() => StartBaseService(IPAddress.Parse("127.0.0.1"), 30_001, UseCodeService, cancellationToken), cancellationToken);

            await Task.WhenAll(tasks);
         
            await _repository.SaveCodes(_codes);
        }

        private static async Task StartBaseService(IPAddress ipAddress, int port, Func<Stream, byte[]> specificService, CancellationToken cancellationToken)
        {
            var listener = new TcpListener(ipAddress, port);
            listener.Start();

            await using (cancellationToken.Register(listener.Stop))
            {
                try
                {
                    while (true)
                    {
                        using var client = await Task.Run(() => listener.AcceptTcpClientAsync(), cancellationToken);
                        await using var stream = await Task.Run(() => client.GetStream(), cancellationToken);

                        var responseAsByteArray = specificService(stream);

                        await stream.WriteAsync(responseAsByteArray, 0, 1, cancellationToken);
                    }
                }
                catch (ObjectDisposedException)
                { }
            }
        }

        private byte[] GenerateService(Stream stream)
        {
            var requestData = _dataEncoder.DecodeGenerateRequest(stream);

            var success = requestData.NumberOfCodesToGenerate > 0
                          && _codes.GenerateNewCodes(requestData.NumberOfCodesToGenerate,
                              requestData.CodeLength);

            return _dataEncoder.EncodeGenerateResponse(success);
        }

        private byte[] UseCodeService(Stream stream)
        {
            var codeToLookUp = _dataEncoder.DecodeUseCodeRequest(stream);

            var response = _codes.CheckCode(codeToLookUp);

            return _dataEncoder.EncodeUseCodeResponse(response);
        }
    }
}
