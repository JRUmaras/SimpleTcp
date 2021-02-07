using System;
using CodeService.Enums;

namespace CodeService.Models
{
    public class Code : IEquatable<Code>
    {
        public string Value { get; }

        public CodeState State { get; private set; }

        public Code(string value, CodeState state = CodeState.DoesNotExist)
        {
            Value = value;
            State = state;
        }

        public void MarkAsUsed() => State = CodeState.Used;

        public bool Equals(Code other) => other != null && string.Equals(Value, other.Value, StringComparison.InvariantCulture);

        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(Value);
        }
    }
}
