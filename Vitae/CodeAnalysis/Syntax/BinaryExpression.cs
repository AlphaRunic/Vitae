using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class NameExpression : ExpressionSyntax
    {
        public NameExpression(Token identifier)
        {
            Identifier = identifier;
        }

        public override SyntaxType Type => SyntaxType.NameExpression;
        public Token Identifier { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Identifier;
        }
    }

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

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Identifier;
            yield return EqualsToken;
            yield return Expression;
        }
    }

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

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}