using System.Linq;
using CodeService.Enums;
using CodeService.Helpers;
using CodeService.Models;
using NUnit.Framework;

namespace TestCodeService
{
    public class UseCodeTests
    {
        [Test]
        public void UseUnusedCodeTest()
        {
            var codes = new CodesCollection();
            var generator = new CodeGenerator();

            var codeLength = 8;
            var numberOfCodesToGenerate = 10;

            var wasSuccess = generator.TryAddNewUniqueCodes(codes, codeLength, numberOfCodesToGenerate, out _);
            Assert.IsTrue(wasSuccess);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);

            var codeState = codes.CheckCode(codes.First());
            Assert.AreEqual(CodeState.NotUsed, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(2, states.Count);
            Assert.AreEqual(numberOfCodesToGenerate - 1, states[CodeState.NotUsed]);
            Assert.AreEqual(1, states[CodeState.Used]);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);
        }

        [Test]
        public void UseUsedCodeTest()
        {
            var codes = new CodesCollection();
            var generator = new CodeGenerator();

            var codeLength = 8;
            var numberOfCodesToGenerate = 10;

            var wasSuccess = generator.TryAddNewUniqueCodes(codes, codeLength, numberOfCodesToGenerate, out _);
            Assert.IsTrue(wasSuccess);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);

            var codeState = codes.CheckCode(codes.First());
            Assert.AreEqual(CodeState.NotUsed, codeState);

            codeState = codes.CheckCode(codes.First());
            Assert.AreEqual(CodeState.Used, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(2, states.Count);
            Assert.AreEqual(numberOfCodesToGenerate - 1, states[CodeState.NotUsed]);
            Assert.AreEqual(1, states[CodeState.Used]);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);
        }

        [Test]
        public void UseNonExistingCode()
        {
            var codeGenerator = new CodeGenerator();
            var codes = new CodesCollection();

            var codeLength = 8;
            var numberOfCodesToGenerate = 10;

            var wasSuccess = codeGenerator.TryAddNewUniqueCodes(codes, codeLength, numberOfCodesToGenerate, out _);
            Assert.IsTrue(wasSuccess);
            Assert.AreEqual(10, codes.Count);

            Code code;
            while (true)
            {
                code = codeGenerator.GenerateCode(codeLength);
                if (!codes.Contains(code)) break;
            }

            var codeState = codes.CheckCode(code);
            Assert.AreEqual(CodeState.DoesNotExist, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(1, states.Count);
            Assert.AreEqual(numberOfCodesToGenerate, states[CodeState.NotUsed]);
            Assert.AreEqual(numberOfCodesToGenerate, codes.Count);
        }

        [Test]
        public void UseCodeWhenCodeCollectionIsEmpty()
        {
            var codeGenerator = new CodeGenerator();
            var codes = new CodesCollection();

            var codeLength = 8;

            var code = codeGenerator.GenerateCode(codeLength);

            var codeState = codes.CheckCode(code);
            Assert.AreEqual(CodeState.DoesNotExist, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(0, states.Count);
            Assert.AreEqual(0, codes.Count);
        }
    }
}
