using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class UnaryExpression : ExpressionSyntax
    {
        public UnaryExpression(Token operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxType Type => SyntaxType.UnaryExpression;
        public Token OperatorToken { get; }
        public ExpressionSyntax Operand { get; }
    }
}