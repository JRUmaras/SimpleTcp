using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var codes = new CodesCollection(new CodeGenerator());

            var wasSuccess = codes.GenerateNewCodes(10, 8);
            Assert.IsTrue(wasSuccess);
            Assert.AreEqual(10, codes.Length);

            var codeState = codes.CheckCode(codes.First());
            Assert.AreEqual(CodeState.NotUsed, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(2, states.Count);
            Assert.AreEqual(9, states[CodeState.NotUsed]);
            Assert.AreEqual(1, states[CodeState.Used]);
            Assert.AreEqual(10, codes.Length);
        }

        [Test]
        public void UseUsedCodeTest()
        {
            var codes = new CodesCollection(new CodeGenerator());

            var wasSuccess = codes.GenerateNewCodes(10, 8);
            Assert.IsTrue(wasSuccess);
            Assert.AreEqual(10, codes.Length);

            var codeState = codes.CheckCode(codes.First());
            Assert.AreEqual(CodeState.NotUsed, codeState);

            codeState = codes.CheckCode(codes.First());
            Assert.AreEqual(CodeState.Used, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(2, states.Count);
            Assert.AreEqual(9, states[CodeState.NotUsed]);
            Assert.AreEqual(1, states[CodeState.Used]);
            Assert.AreEqual(10, codes.Length);
        }

        [Test]
        public void UseNonExistingCode()
        {
            var codeGenerator = new CodeGenerator();
            var codes = new CodesCollection(codeGenerator);

            var wasSuccess = codes.GenerateNewCodes(10, 8);
            Assert.IsTrue(wasSuccess);
            Assert.AreEqual(10, codes.Length);

            Code code;
            while (true)
            {
                code = codeGenerator.GenerateCode(8);
                if (!codes.Contains(code)) break;
            }

            var codeState = codes.CheckCode(code);
            Assert.AreEqual(CodeState.DoesNotExist, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(1, states.Count);
            Assert.AreEqual(10, states[CodeState.NotUsed]);
            Assert.AreEqual(10, codes.Length);
        }

        [Test]
        public void UseCodeWhenCodeCollectionIsEmpty()
        {
            var codeGenerator = new CodeGenerator();
            var codes = new CodesCollection(codeGenerator);

            var code = codeGenerator.GenerateCode(8);

            var codeState = codes.CheckCode(code);
            Assert.AreEqual(CodeState.DoesNotExist, codeState);

            var states = codes.GroupBy(c => c.State).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(0, states.Count);
            Assert.AreEqual(0, codes.Length);
        }
    }
}
