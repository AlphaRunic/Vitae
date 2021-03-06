using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class BinaryExpression : ExpressionSyntax
    {
        public BinaryExpression(ExpressionSyntax left, Token operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxType Type => SyntaxType.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public Token OperatorToken { get; }
        public ExpressionSyntax Right { get; }
    }
}