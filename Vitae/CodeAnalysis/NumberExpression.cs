using System.Collections.Generic;

namespace Vitae.CodeAnalysis
{
    public sealed class NumberExpression : ExpressionSyntax
    {
        public NumberExpression(Token token)
        {
            Token = token;
        }

        public override SyntaxType Type => SyntaxType.NumberExpression;
        public Token Token { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}