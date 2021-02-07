using System.Collections;
using System.Collections.Generic;
using CodeService.Enums;
using CodeService.Interfaces;

namespace CodeService.Models
{
    public class CodesCollection : ICodesCollection
    {
        private readonly HashSet<Code> _codes = new HashSet<Code>();

        public int Count => _codes.Count;

        public bool Contains(Code code) => _codes.Contains(code);

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

        public IEnumerator<Code> GetEnumerator() => _codes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(IEnumerable<Code> codes) => _codes.UnionWith(codes);

        public void RemoveRange(IEnumerable<Code> codes) => _codes.ExceptWith(codes);

        public bool Add(Code codes) => _codes.Add(codes);

        public void Remove(Code codes) => _codes.Remove(codes);
    }
}
