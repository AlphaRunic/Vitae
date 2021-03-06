using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class ParenthesizedExpression : ExpressionSyntax
    {
        public ParenthesizedExpression(Token openParen, ExpressionSyntax expression, Token closedParen)
        {
            OpenParen = openParen;
            Expression = expression;
            ClosedParen = closedParen;
        }

        public Token OpenParen { get; }
        public ExpressionSyntax Expression { get; }
        public Token ClosedParen { get; }

        public override SyntaxType Type => SyntaxType.ParenthesizedExpression;
    }
}