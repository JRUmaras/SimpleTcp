using System.IO;
using System.Text;
using CodeService.Enums;
using CodeService.Helpers;
using NUnit.Framework;

namespace TestCodeService
{
    public class DataEncoderUseCodeTests
    {
        [Test]
        [TestCase("Ab1Xy234")]
        [TestCase("123asdQ")]
        [TestCase("ac5")]
        public void NormalInputTest(string codeValue)
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();
            
            var codeValueAsBytes = Encoding.ASCII.GetBytes(codeValue);

            memStream.Write(codeValueAsBytes, 0, codeValueAsBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var code = encoder.DecodeUseCodeRequest(memStream);
            Assert.AreEqual(codeValue, code.Value);
            memStream.SetLength(0);
        }

        [Test]
        [TestCase(CodeState.DoesNotExist, (byte)CodeState.DoesNotExist)]
        [TestCase(CodeState.Used, (byte)CodeState.Used)]
        [TestCase(CodeState.NotUsed, (byte)CodeState.NotUsed)]
        public void NormalOutputTest(CodeState state, byte expectedByte)
        {
            var output = new DataEncoder().EncodeUseCodeResponse(state);
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual(expectedByte, output[0]);
        }

        
    }
}
