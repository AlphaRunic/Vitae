using System;
using Vitae.CodeAnalysis.Binding;
using Vitae.CodeAnalysis.Syntax;

namespace Vitae.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteralExpression n)
                return n.Value;

            if (node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.Op.Type)
                {
                    case BoundUnaryOperatorType.Identity:
                        return (int) MathF.Abs((int) operand);
                    case BoundUnaryOperatorType.Negation:
                        return -(int) operand;
                    case BoundUnaryOperatorType.LogicalNegation:
                        return !(bool) operand;
                        
                    default:
                        throw new Exception($"unexpected unary operator {u.Op}");
                }
            }

            if (node is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                
                switch (b.Op.Type) {
                    case BoundBinaryOperatorType.Addition:
                        return (int) left + (int) right;
                    case BoundBinaryOperatorType.Subtraction:
                        return (int) left - (int) right;
                    case BoundBinaryOperatorType.Multiplication:
                        return (int) left * (int) right;
                    case BoundBinaryOperatorType.Division:
                        return (int) left / (int) right;
                    case BoundBinaryOperatorType.Exponentation:
                        return (int) MathF.Pow((int) left, (int) right);
                    case BoundBinaryOperatorType.Modulus:
                        return (int) left % (int) right;

                    case BoundBinaryOperatorType.LogicalAnd:
                        return (bool) left && (bool) right;
                    case BoundBinaryOperatorType.LogicalOr:
                        return (bool) left || (bool) right;

                    default:
                        throw new Exception($"unexpected binary operator {b.Op}");
                }
            }

            throw new Exception($"unexpected node {node.Type}");
        }
    }
}