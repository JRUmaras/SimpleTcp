﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CodeService.Enums;
using CodeService.Interfaces;
using CodeService.Models;

namespace CodeService
{
    public class CodeService : ICodeService
    {
        private readonly IDataEncoder _dataEncoder;
        private readonly ICodesCollection _codes;
        private readonly IRepository _repository;
        private readonly ICodeGenerator _codeGenerator;

        private const string _ip = "127.0.0.1";
        private const int _portForGeneratingCodes = 30_000;
        private const int _portForUsingCode = 30_001;

        public CodeService(IDataEncoder dataEncoder, ICodesCollection codesCollection, IRepository repository, ICodeGenerator codeGenerator)
        {
            _dataEncoder = dataEncoder;
            _codes = codesCollection;
            _repository = repository;
            _codeGenerator = codeGenerator;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            _codes.AddRange(await Task.Run(_repository.LoadCodes, cancellationToken));

            var tasks = new Task[2];

            tasks[0] = Task.Run(() => StartBaseService(IPAddress.Parse(_ip), _portForGeneratingCodes, GenerateService, cancellationToken), cancellationToken);
            tasks[1] = Task.Run(() => StartBaseService(IPAddress.Parse(_ip), _portForUsingCode, UseCodeService, cancellationToken), cancellationToken);

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

            var success = TryGenerateNewCodes(requestData);

            return _dataEncoder.EncodeGenerateResponse(success);
        }

        private byte[] UseCodeService(Stream stream)
        {
            var codeToLookUp = _dataEncoder.DecodeUseCodeRequest(stream);

            var response = _codes.CheckCode(codeToLookUp);

            // In UseCode operation, the only case when any code changes its
            // state is when an unused code is checked. So we save 
            // the codes in the storage only in such a case.
            if (response == CodeState.NotUsed) SaveCodesToStorage();

            return _dataEncoder.EncodeUseCodeResponse(response);
        }

        private bool SaveCodesToStorage()
        {
            var task = _repository.SaveCodes(_codes);

            Task.WaitAll(task);

            return task.Result;
        }

        private bool TryGenerateNewCodes(GenerationParameters requestData)
        {
            var generationSucceeded = _codeGenerator.TryAddNewUniqueCodes(_codes, requestData.CodeLength, requestData.NumberOfCodesToGenerate, out var newCodes);

            if (!generationSucceeded) return false;

            if (SaveCodesToStorage()) return true;

            // Rollback if save to storage failed.
            _codes.RemoveRange(newCodes);
            return false;
        }
    }
}
