using System.Collections.Generic;
using CodeService.Enums;
using CodeService.Models;

namespace CodeService.Interfaces
{
    public interface ICodesCollection : IEnumerable<Code>
    {
        int Length { get; }

        bool GenerateNewCodes(int numberOfCodesToAdd, int codeLength);

        CodeState CheckCode(Code codeToLookUp);

        void AddRange(IEnumerable<Code> codes);
    }
}
