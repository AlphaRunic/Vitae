using System;
using System.Collections.Generic;

using Vitae.CodeAnalysis.Binding;

namespace Vitae.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            switch (node.NodeType)
            {
                case BoundNodeType.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression) node);
                case BoundNodeType.VariableExpression:
                    return EvaluateVariableExpression((BoundVariableExpression) node);
                case BoundNodeType.AssignmentExpression:
                    return EvaluateAssignmentExpression((BoundAssignmentExpression) node);
                case BoundNodeType.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression) node);
                case BoundNodeType.BinaryExpression:
                    return EvaluateBinaryExpression((BoundBinaryExpression) node);
            }

            throw new Exception($"unexpected node {node.Type}");
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            switch (b.Op.Type)
            {
                case BoundBinaryOperatorType.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorType.Subtraction:
                    return (int)left - (int)right;
                case BoundBinaryOperatorType.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorType.Division:
                    return (int)left / (int)right;
                case BoundBinaryOperatorType.Exponentation:
                    return (int)Math.Pow((int)left, (int)right);
                case BoundBinaryOperatorType.Modulus:
                    return (int)left % (int)right;

                case BoundBinaryOperatorType.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorType.LogicalOr:
                    return (bool)left || (bool)right;

                case BoundBinaryOperatorType.Equals:
                    return Equals(left, right);
                case BoundBinaryOperatorType.NotEquals:
                    return !Equals(left, right);

                default:
                    throw new Exception($"unexpected binary operator {b.Op}");
            }
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            switch (u.Op.Type)
            {
                case BoundUnaryOperatorType.Identity:
                    return (int)Math.Abs((int)operand);
                case BoundUnaryOperatorType.Negation:
                    return -(int)operand;
                case BoundUnaryOperatorType.LogicalNegation:
                    return !(bool)operand;

                default:
                    throw new Exception($"unexpected unary operator {u.Op}");
            }
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a)
        {
            var value = EvaluateExpression(a.Expression);
            _variables[a.Variable] = value;
            return value;
        }

        private object EvaluateVariableExpression(BoundVariableExpression v)
        {
            return _variables[v.Variable];
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }
    }
}