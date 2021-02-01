using System;
using CodeService.Helpers;
using NUnit.Framework;

namespace TestCodeService
{
    public class SingleCodeGenerationTests
    {
        [Test]
        public void CodeIsGeneratedTest()
        {
            var generator = new CodeGenerator();

            var code = generator.GenerateCode(0);
            Assert.IsNotNull(code);
        }

        [Test]
        [TestCase(1)]
        [TestCase(7)]
        [TestCase(8)]
        public void NormalCodeGenerationTest(int length)
        {
            var generator = new CodeGenerator();

            var code = generator.GenerateCode(length);
            Assert.IsNotNull(code);
            Assert.AreEqual(length, code.Value.Length);
        }

        [Test]
        public void NegativeLengthCodeRequestTest()
        {
            var generator = new CodeGenerator();

            Assert.Throws<OverflowException>(() => generator.GenerateCode(-2));
        }
    }
}
