using System.IO;
using System.Security.Cryptography;
using CodeService.Helpers;
using CodeService.Models;
using NUnit.Framework;

namespace TestCodeService
{
    public class DataEncoderGenerationTests
    {
        [Test]
        [TestCase(new byte[] { 0, 255, 8 }, 8, 255)]
        [TestCase(new byte[] { 1, 0, 7 }, 7, 256)]
        [TestCase(new byte[] { 7, 208, 7 }, 7, 2000)]
        [TestCase(new byte[] { 4, 12, 7 }, 7, 1036)]
        public void NormalInputTest(byte[] input, int expectedCodeLength, int expectedNumberOfCodesToGenerate)
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.Write(input, 0, input.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(expectedCodeLength, requestData.CodeLength);
        }

        [Test]
        public void TooManyCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.Write(new byte[] { 7, 209, 7 }, 0, 3);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
        }

        [Test]
        public void ZeroCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.Write(new byte[] { 0, 0, 7 }, 0, 3);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(7, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
        }

        [Test]
        public void TooLongCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.Write(new byte[] { 0, 1, 9 }, 0, 3);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(9, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
        }

        [Test]
        public void TooShortCodesRequestedTest()
        {
            var encoder = new DataEncoder();

            var memStream = new MemoryStream();

            memStream.Write(new byte[] { 0, 1, 6 }, 0, 3);
            memStream.Seek(0, SeekOrigin.Begin);
            var requestData = encoder.DecodeGenerateRequest(memStream);
            Assert.AreEqual(6, requestData.CodeLength);
            Assert.AreEqual(0, requestData.NumberOfCodesToGenerate);
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