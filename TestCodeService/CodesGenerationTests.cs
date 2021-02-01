using System.Collections.Generic;
using System.Linq;
using CodeService.Helpers;
using CodeService.Models;
using NUnit.Framework;

namespace TestCodeService
{
    public class CodesGenerationTests
    {
        [Test]
        [TestCase(8, 100)]
        [TestCase(7, 3000)]
        [TestCase(8, 2000)]
        public void GenerateAndAddCodesToHashSetTest(int codeLength, int numberOfCodesToGenerate)
        {
            var generator = new CodeGenerator();
            var codes = new HashSet<Code>();

            var wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, codeLength, numberOfCodesToGenerate);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);
            Assert.IsTrue(wasSuccessful);
        }

        [Test]
        public void CodePseudoUniquenessTest()
        {
            var generator = new CodeGenerator();
            var codes = new HashSet<Code>();

            var wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 8, 2_000_000);
            Assert.IsTrue(wasSuccessful);

            wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 8, 3_000_000);
            Assert.IsTrue(wasSuccessful);

            Assert.AreEqual(5_000_000, codes.Select(c => c.Value).Distinct().Count());
        }

        [Test]
        public void CodesGenerationAttemptsCeilingTest()
        {
            var generator = new CodeGenerator();
            var codes = new HashSet<Code>();

            var wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 0, 1);
            Assert.AreEqual(1, codes.Count);
            Assert.IsTrue(wasSuccessful);

            wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 0, 1);
            Assert.AreEqual(1, codes.Count);
            Assert.IsFalse(wasSuccessful);
        }
    }
}
