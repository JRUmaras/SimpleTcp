using System.Collections.Generic;
using System.Linq;
using CodeService.Enums;
using CodeService.Helpers;
using CodeService.Models;
using NUnit.Framework;

namespace TestCodeService
{
    public class CodesGenerationTests
    {
        [Test]
        public void GenerateAndAddCodesToHashSetTest()
        {
            var generator = new CodeGenerator();
            var codes = new HashSet<Code>();

            var wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 8, 100);
            Assert.AreEqual(100, codes.Count);
            Assert.IsTrue(wasSuccessful);

            wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 7, 3000);
            Assert.AreEqual(3100, codes.Count);
            Assert.IsTrue(wasSuccessful);

            wasSuccessful = generator.GenerateAndAddUniqueCodes(codes, 8, 2000);
            Assert.AreEqual(5100, codes.Count);
            Assert.IsTrue(wasSuccessful);
        }

        [Test]
        public void CodeUniquenessTest()
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

        [Test]
        public void HashSetOfCodesTest()
        {
            var set = new HashSet<Code>();

            var code1 = new Code("ABC");
            var code2 = new Code("CBA");
            var code3 = new Code("ABC");
            var code4 = new Code("ABC", CodeState.Used);

            var res = set.Add(code1);
            Assert.IsTrue(res);

            res = set.Add(code2);
            Assert.IsTrue(res);

            res = set.Add(code3);
            Assert.IsFalse(res);

            res = set.Add(code1);
            Assert.IsFalse(res);

            res = set.Add(code4);
            Assert.IsFalse(res);

            Assert.AreEqual(2, set.Count);
        }
    }
}
