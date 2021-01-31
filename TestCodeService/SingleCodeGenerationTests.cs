using System;
using CodeService.Helpers;
using NUnit.Framework;

namespace TestCodeService
{
    public class SingleCodeGenerationTests
    {
        [Test]
        public void AssertNonNullCodeIsGeneratedTest()
        {
            var generator = new CodeGenerator();

            var code = generator.GenerateCode(0);
            Assert.IsNotNull(code);
        }

        [Test]
        public void NormalCodeGenerationTest()
        {
            var generator = new CodeGenerator();

            var code = generator.GenerateCode(1);
            Assert.IsNotNull(code);
            Assert.AreEqual(1, code.Value.Length);

            code = generator.GenerateCode(7);
            Assert.IsNotNull(code);
            Assert.AreEqual(7, code.Value.Length);

            code = generator.GenerateCode(8);
            Assert.IsNotNull(code);
            Assert.AreEqual(8, code.Value.Length);
        }

        [Test]
        public void NegativeLengthCodeRequestTest()
        {
            var generator = new CodeGenerator();

            Assert.Throws<OverflowException>(() => generator.GenerateCode(-2));
        }
    }
}
