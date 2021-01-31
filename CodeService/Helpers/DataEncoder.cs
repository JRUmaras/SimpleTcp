using System.IO;
using System.Linq;
using CodeService.Enums;
using CodeService.Interfaces;
using CodeService.Models;

namespace CodeService.Helpers
{
    public class DataEncoder : IDataEncoder
    {
        private const int _bufferSizeForInputGenerate = 3;
        private const int _bufferSizeForInputUseCode = 8;
        private const int _maxCodeLength = 8;
        private const int _bitsInByte = 8;
        private const int _maxNumberOfCodesToGenerate = 2_000;

        private readonly int[] _validCodeLengths = {7, 8};

        public GenerationParameters DecodeGenerateRequest(Stream stream)
        {
            var buffer = new byte[_bufferSizeForInputGenerate];
            stream.Read(buffer, 0, _bufferSizeForInputGenerate);

            var codeLength = (int) buffer[2];
            var numberOfCodesToGenerate = (buffer[0] << _bitsInByte) + buffer[1];

            if (!_validCodeLengths.Contains(codeLength) || numberOfCodesToGenerate > _maxNumberOfCodesToGenerate || numberOfCodesToGenerate < 1)
            {
                numberOfCodesToGenerate = 0;
            }

            return new GenerationParameters
            {
                CodeLength = codeLength,
                NumberOfCodesToGenerate =  numberOfCodesToGenerate
            };
        }

        public byte[] EncodeGenerateResponse(bool value) => new[] { (byte)(value ? 1 : 0) };

        public Code DecodeUseCodeRequest(Stream stream)
        {
            var buffer = new byte[_bufferSizeForInputUseCode];
            var charRead = stream.Read(buffer, 0, _bufferSizeForInputUseCode);

            var codeValue = System.Text.Encoding.ASCII.GetString(buffer, 0, charRead);

            return new Code(codeValue);
        }

        public byte[] EncodeUseCodeResponse(CodeState value) => new[] { (byte)value };
    }
}
