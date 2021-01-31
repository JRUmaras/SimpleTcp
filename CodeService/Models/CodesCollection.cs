using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeService.Enums;
using CodeService.Interfaces;

namespace CodeService.Models
{
    public class CodesCollection : ICodesCollection
    {
        private readonly HashSet<Code> _codes = new HashSet<Code>();

        private readonly ICodeGenerator _codeGenerator;

        public int Length => _codes.Count;

        public CodesCollection(ICodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        public bool GenerateNewCodes(int numberOfCodesToAdd, int codeLength)
        {
            var wasSuccessful = _codeGenerator.GenerateAndAddUniqueCodes(_codes, codeLength, numberOfCodesToAdd);

            return wasSuccessful;
        }

        public bool Contains(Code code)
        {
            return _codes.Contains(code);
        }

        public CodeState CheckCode(Code codeToLookUp)
        {
            CodeState response;

            if (_codes.TryGetValue(codeToLookUp, out var code))
            {
                response = code.State;
                code.MarkAsUsed();
            }
            else
            {
                response = CodeState.DoesNotExist;
            }

            return response;
        }

        public IEnumerator<Code> GetEnumerator()
        {
            return _codes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddRange(IEnumerable<Code> codes)
        {
            foreach (var code in codes) _codes.Add(code);
        }
    }
}
