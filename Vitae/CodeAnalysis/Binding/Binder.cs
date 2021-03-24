using System;
using System.Collections.Generic;
using Vitae.CodeAnalysis.Syntax;

namespace Vitae.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly List<string> _diagnostics = new List<string>();

        public Binder()
        {
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Type)
            {
                case SyntaxType.LiteralExpression:
                    return BindLiteralExpression((LiteralExpression) syntax);
                case SyntaxType.UnaryExpression:
                    return BindUnaryExpression((UnaryExpression) syntax);
                case SyntaxType.BinaryExpression:
                    return BindBinaryExpression((BinaryExpression) syntax);
                default:
                    throw new Exception($"unexpected syntax {syntax.Type}");
                    
            }
        }

        private BoundExpression BindUnaryExpression(UnaryExpression syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Type, boundOperand.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Type, boundLeft.Type, boundRight.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }

        private BoundExpression BindLiteralExpression(LiteralExpression syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
    }
}