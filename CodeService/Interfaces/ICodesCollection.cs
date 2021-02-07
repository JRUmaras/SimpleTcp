using System.Collections.Generic;
using CodeService.Enums;
using CodeService.Models;

namespace CodeService.Interfaces
{
    public interface ICodesCollection : IEnumerable<Code>
    {
        int Count { get; }

        CodeState CheckCode(Code codeToLookUp);

        bool Add(Code code);

        void Remove(Code code);

        void AddRange(IEnumerable<Code> codes);

        void RemoveRange(IEnumerable<Code> codes);
    }
}
