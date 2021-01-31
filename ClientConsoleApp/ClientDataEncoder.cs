using System.IO;
using System.Text;
using CodeService.Enums;

namespace ClientConsoleApp
{
    public class ClientDataEncoder
    {
        private const int _bitsInByte = 8;

        public static byte[] EncodeGenerateRequest(ushort numberOfCodesToGenerate, int codeLength)
        {
            return new [] { (byte)((numberOfCodesToGenerate >> _bitsInByte) & byte.MaxValue), (byte)(numberOfCodesToGenerate & byte.MaxValue), (byte)codeLength };
        }

        public static bool DecodeGenerateResponse(Stream stream)
        {
            var buffer = new byte[1];

            stream.Read(buffer, 0, 1);

            return buffer[0] > 0;
        }

        public static CodeState DecodeUseCodeResponse(Stream stream)
        {
            return (CodeState) stream.ReadByte();
        }

        public static byte[] EncodeUseCodeRequest(string codeValue)
        {
            return Encoding.ASCII.GetBytes(codeValue);  
        }
    }
}
