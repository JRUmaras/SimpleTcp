using System.Net;
using System.Net.Sockets;
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

        public void Start()
        {
            _codes.AddRange(Task.Run(_repository.LoadCodes).Result);

            var tasks = new Task[2];

            tasks[0] = Task.Run(StartGeneratorService);
            tasks[1] = Task.Run(StartUseCodeService);

            Task.WaitAll(tasks);
        }

        public async Task StartGeneratorService()
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 30_000);
            listener.Start();

            while (true)
            {
                using var client = await listener.AcceptTcpClientAsync();

                await using var stream = client.GetStream();

                var requestData = _dataEncoder.DecodeGenerateRequest(stream);

                var success = requestData.NumberOfCodesToGenerate > 0 
                              && _codes.GenerateNewCodes(requestData.NumberOfCodesToGenerate, requestData.CodeLength);

                var responseAsByteArray = _dataEncoder.EncodeGenerateResponse(success);

                stream.Write(responseAsByteArray, 0, 1);

                var wasSaved = await _repository.SaveCodes(_codes);
            }
        }

        public async Task StartUseCodeService()
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 30_001);
            listener.Start();

            while (true)
            {
                using var client = await listener.AcceptTcpClientAsync();

                await using var stream = client.GetStream();

                var codeToLookUp = _dataEncoder.DecodeUseCodeRequest(stream);

                var response = _codes.CheckCode(codeToLookUp);

                var responseAsByteArray = _dataEncoder.EncodeUseCodeResponse(response);

                stream.Write(responseAsByteArray, 0, 1);

                var wasSaved = await _repository.SaveCodes(_codes);
            }
        }
    }
}
