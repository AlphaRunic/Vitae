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

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteralExpression n)
                return (int) n.Value;

            if (node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.OperatorType)
                {
                    case BoundUnaryOperatorType.Identity:
                        return (int) MathF.Abs(operand);
                    case BoundUnaryOperatorType.Negation:
                        return -operand;
                        
                    default:
                        throw new Exception($"unexpected unary operator {u.OperatorType}");
                }
            }

            if (node is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                
                switch (b.OperatorType) {
                    case BoundBinaryOperatorType.Addition:
                        return left + right;
                    case BoundBinaryOperatorType.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorType.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorType.Division:
                        return left / right;
                    case BoundBinaryOperatorType.Exponentation:
                        return (int) MathF.Pow(left, right);
                    case BoundBinaryOperatorType.Modulus:
                        return left % right;
                    default:
                        throw new Exception($"unexpected binary operator {b.OperatorType}");
                }
            }

            throw new Exception($"unexpected node {node.Type}");
        }
    }
}