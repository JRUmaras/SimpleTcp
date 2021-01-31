using System.IO;
using CodeService.Helpers;
using NUnit.Framework;

namespace TestCodeService
{
    public class DataEncoderGenerationTests
    {
        [Test]
        public void NormalInputTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.WriteByte(0);
            memStream.WriteByte(255);
            memStream.WriteByte(8);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(8, requestData.CodeLength);
            Assert.AreEqual(255, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);

            memStream.WriteByte(1);
            memStream.WriteByte(0);
            memStream.WriteByte(7);
            memStream.Seek(0, SeekOrigin.Begin);
            requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(256, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);

            memStream.WriteByte(7);
            memStream.WriteByte(208);
            memStream.WriteByte(7);
            memStream.Seek(0, SeekOrigin.Begin);
            requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(2000, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);

            memStream.WriteByte(4);
            memStream.WriteByte(12);
            memStream.WriteByte(7);
            memStream.Seek(0, SeekOrigin.Begin);
            requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(1036, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);
        }

        [Test]
        public void TooManyCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.WriteByte(7);
            memStream.WriteByte(209);
            memStream.WriteByte(7);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);
        }

        [Test]
        public void ZeroCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.WriteByte(0);
            memStream.WriteByte(0);
            memStream.WriteByte(7);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);
        }

        [Test]
        public void TooLongCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.WriteByte(0);
            memStream.WriteByte(1);
            memStream.WriteByte(9);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(9, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);
        }

        [Test]
        public void TooShortCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.WriteByte(0);
            memStream.WriteByte(1);
            memStream.WriteByte(6);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(6, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
            memStream.SetLength(0);
        }

        [Test]
        public void NormalOutputTest()
        {
            var encoder = new DataEncoder();

            Assert.AreEqual(1, encoder.EncodeGenerateResponse(true).Length);
            Assert.AreEqual(1, encoder.EncodeGenerateResponse(true)[0]);

            Assert.AreEqual(1, encoder.EncodeGenerateResponse(false).Length);
            Assert.AreEqual(0, encoder.EncodeGenerateResponse(false)[0]);
        }
    }
}