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
            var codes = new CodesCollection();

            var wasSuccessful = generator.TryAddNewUniqueCodes(codes, codeLength, numberOfCodesToGenerate, out var newCodes);

            Assert.AreEqual(numberOfCodesToGenerate, newCodes.Count);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);
            Assert.IsTrue(wasSuccessful);
        }

        [Test]
        public void CodePseudoUniquenessTest()
        {
            var generator = new CodeGenerator();
            var codes = new CodesCollection();

            var numberOfCodesToGenerateFirstTime = 10_000_000;

            var wasSuccessful = generator.TryAddNewUniqueCodes(codes, 8, numberOfCodesToGenerateFirstTime, out var newCodes);
            Assert.AreEqual(numberOfCodesToGenerateFirstTime, newCodes.Count);
            Assert.AreEqual(numberOfCodesToGenerateFirstTime, codes.Count);
            Assert.IsTrue(wasSuccessful);

            var numberOfCodesToGenerateSecondTime = 3_000_000;

            wasSuccessful = generator.TryAddNewUniqueCodes(codes, 8, numberOfCodesToGenerateSecondTime, out newCodes);
            Assert.AreEqual(numberOfCodesToGenerateSecondTime, newCodes.Count);
            Assert.AreEqual(numberOfCodesToGenerateFirstTime + numberOfCodesToGenerateSecondTime, codes.Count);
            Assert.IsTrue(wasSuccessful);

            Assert.AreEqual(numberOfCodesToGenerateFirstTime + numberOfCodesToGenerateSecondTime, codes.Select(c => c.Value).Distinct().Count());
        }

        [Test]
        public void CodesGenerationAttemptsCeilingTest()
        {
            var generator = new CodeGenerator();
            var codes = new CodesCollection();

            var wasSuccessful = generator.TryAddNewUniqueCodes(codes, 0, 1, out _);
            Assert.AreEqual(1, codes.Count);
            Assert.IsTrue(wasSuccessful);

            wasSuccessful = generator.TryAddNewUniqueCodes(codes, 0, 1, out _);
            Assert.AreEqual(1, codes.Count);
            Assert.IsFalse(wasSuccessful);
        }
    }
}
