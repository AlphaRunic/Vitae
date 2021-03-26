using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class AssignmentExpression : ExpressionSyntax
    {
        public AssignmentExpression(Token identifier, Token equalsToken, ExpressionSyntax expression)
        {
            Identifier = identifier;
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override SyntaxType Type => SyntaxType.AssignmentExpression;
        public Token Identifier { get; }
        public Token EqualsToken { get; }
        public ExpressionSyntax Expression { get; }
    }
}