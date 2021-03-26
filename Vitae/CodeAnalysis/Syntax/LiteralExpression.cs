using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public LiteralExpression(Token token)
            : this(token, token.Value)
        {
        }

        public LiteralExpression(Token token, object value)
        {
            Token = token;
            Value = value;
        }

        public override SyntaxType Type => SyntaxType.LiteralExpression;
        public Token Token { get; }
        public object Value { get; }
    }
}