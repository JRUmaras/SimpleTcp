using System.IO;
using NUnit.Framework;
using ClientConsoleApp;

namespace TestClientConsoleApp
{
    public class ClientDataEncoderTests
    {
        [Test]
        [TestCase(new byte[] { 7, 208, 8 }, (ushort)2000, 8)]
        [TestCase(new byte[] { 3, 232, 7 }, (ushort)1000, 7)]
        [TestCase(new byte[] { 0, 50, 8 }, (ushort)50, 8)]
        [TestCase(new byte[] { 0, 0, 8 }, (ushort)0, 8)]
        public void EncodingTest(byte[] expectedOutput, ushort numberOfCodesToGenerate, int codeLength)
        {
            var requestAsBytes = ClientDataEncoder.EncodeGenerateRequest(numberOfCodesToGenerate, codeLength);
            Assert.AreEqual(expectedOutput[0], requestAsBytes[0]);
            Assert.AreEqual(expectedOutput[1], requestAsBytes[1]);
            Assert.AreEqual(expectedOutput[2], requestAsBytes[2]);
        }


        [Test]
        [TestCase((byte)0, false)]
        [TestCase((byte)1, true)]
        public void DecodingTest(byte input, bool expectedOutput)
        {
            var memStream = new MemoryStream();
            memStream.WriteByte(input);
            memStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(expectedOutput, ClientDataEncoder.DecodeGenerateResponse(memStream));
        }

        [Test]
        [TestCase(new byte[] { 1, 0 }, true)]
        [TestCase(new byte[] { 0, 1 }, false)]
        public void DecodingTooManyBytesTest(byte[] input, bool expectedOutput)
        {
            var memStream = new MemoryStream();

            memStream.Write(input, 0, input.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(expectedOutput, ClientDataEncoder.DecodeGenerateResponse(memStream));
        }
    }
}