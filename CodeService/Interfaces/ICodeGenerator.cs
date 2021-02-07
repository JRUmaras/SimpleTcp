using System.Collections.Generic;
using CodeService.Models;

namespace CodeService.Interfaces
{
    public interface ICodeGenerator
    {
        bool TryAddNewUniqueCodes(ICodesCollection existingCodesEnumerable, int codeLength, int numberOfCodesToGenerate, out List<Code> newCodesAdded);

        Code GenerateCode(int length);
    }
}
