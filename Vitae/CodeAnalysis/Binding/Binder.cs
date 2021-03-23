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
            var boundOperatorType = BindUnaryOperatorType(syntax.OperatorToken.Type, boundOperand.Type);

            if (boundOperatorType == null)
            {
                _diagnostics.Add($"unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperatorType.Value, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperatorType = BindBinaryOperatorType(syntax.OperatorToken.Type, boundLeft.Type, boundRight.Type);

            if (boundOperatorType == null)
            {
                _diagnostics.Add($"binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperatorType.Value, boundRight);
        }

        private BoundUnaryOperatorType? BindUnaryOperatorType(SyntaxType type, Type operandType)
        {
            if (operandType != typeof(int))
                return null;

            switch (type)
            {
                case SyntaxType.Plus:
                    return BoundUnaryOperatorType.Identity;
                case SyntaxType.Minus:
                    return BoundUnaryOperatorType.Negation;
                
                default:
                    throw new Exception($"unexpected unary operator {type}");
            }
        }

        private BoundBinaryOperatorType? BindBinaryOperatorType(SyntaxType type, Type leftType, Type rightType)
        {
            if (leftType != typeof(int) || rightType != typeof(int))
                return null;

            switch (type)
            {
                case SyntaxType.Plus:
                    return BoundBinaryOperatorType.Addition;
                case SyntaxType.Minus:
                    return BoundBinaryOperatorType.Subtraction;
                case SyntaxType.Multiply:
                    return BoundBinaryOperatorType.Multiplication;
                case SyntaxType.Divide:
                    return BoundBinaryOperatorType.Division;
                case SyntaxType.Power:
                    return BoundBinaryOperatorType.Exponentation;
                case SyntaxType.Modulo:
                    return BoundBinaryOperatorType.Modulus;
                
                default:
                    throw new Exception($"unexpected binary operator {type}");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpression syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
    }
}