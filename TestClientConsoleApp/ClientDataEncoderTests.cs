using System.IO;
using NUnit.Framework;
using ClientConsoleApp;

namespace TestClientConsoleApp
{
    public class ClientDataEncoderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EncodingTest()
        {
            var requestAsBytes = ClientDataEncoder.EncodeGenerateRequest(2000, 8);
            Assert.AreEqual(7, requestAsBytes[0]);
            Assert.AreEqual(208, requestAsBytes[1]);
            Assert.AreEqual(8, requestAsBytes[2]);

            requestAsBytes = ClientDataEncoder.EncodeGenerateRequest(1000, 7);
            Assert.AreEqual(0b0000_0011, requestAsBytes[0]);
            Assert.AreEqual(0b1110_1000, requestAsBytes[1]);
            Assert.AreEqual(7, requestAsBytes[2]);

            requestAsBytes = ClientDataEncoder.EncodeGenerateRequest(50, 8);
            Assert.AreEqual(0, requestAsBytes[0]);
            Assert.AreEqual(0b0011_0010, requestAsBytes[1]);
            Assert.AreEqual(8, requestAsBytes[2]);

            requestAsBytes = ClientDataEncoder.EncodeGenerateRequest(0, 8);
            Assert.AreEqual(0, requestAsBytes[0]);
            Assert.AreEqual(0, requestAsBytes[1]);
            Assert.AreEqual(8, requestAsBytes[2]);
        }


        [Test]
        public void DecodingTest()
        {
            var memStream = new MemoryStream();
            memStream.WriteByte(0);
            memStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(false, ClientDataEncoder.DecodeGenerateResponse(memStream));
            memStream.SetLength(0);

            memStream.WriteByte(1);
            memStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(true, ClientDataEncoder.DecodeGenerateResponse(memStream));
            memStream.SetLength(0);

            memStream.WriteByte(1);
            memStream.WriteByte(0);
            memStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(true, ClientDataEncoder.DecodeGenerateResponse(memStream));
            memStream.SetLength(0);

            memStream.WriteByte(0);
            memStream.WriteByte(1);
            memStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(false, ClientDataEncoder.DecodeGenerateResponse(memStream));
            memStream.SetLength(0);
        }
    }
}