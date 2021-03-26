using System.Collections.Generic;
using System.Linq;
using Vitae.CodeAnalysis.Text;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class Token : SyntaxNode
    {
        public override  TextSpan Span => new TextSpan(Position, Text.Length);
        public override SyntaxType Type { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public Token(SyntaxType type, int pos, string text, object value) {
            Type = type;
            Position = pos;
            Text = text;
            Value = value;
        }
    }
}
