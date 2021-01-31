using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
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

        public bool Equals(Code other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Value, other.Value, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Code) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(Value);
        }
    }
}
