using System.Collections.Generic;

namespace Vitae.CodeAnalysis
{
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public LiteralExpression(Token token)
        {
            Token = token;
        }

        public override SyntaxType Type => SyntaxType.LiteralExpression;
        public Token Token { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}