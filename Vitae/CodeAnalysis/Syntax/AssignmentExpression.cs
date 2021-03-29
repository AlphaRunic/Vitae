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

    public sealed class CompilationUnitSyntax : SyntaxNode
    {
        public CompilationUnitSyntax(ExpressionSyntax expr, Token eofToken)
        {
            Expression = expr;
            EofToken = eofToken;
        }

        public ExpressionSyntax Expression { get; }
        public Token EofToken { get; }
        public override SyntaxType Type => SyntaxType.CompilationUnit;
    }
}