using System;

namespace Vitae.CodeAnalysis
{
    public class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            if (node is NumberExpression n)
                return (int) n.Token.Value;

            if (node is BinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                
                switch (b.OperatorToken.Type) {
                    case SyntaxType.Plus:
                        return left + right;
                    case SyntaxType.Minus:
                        return left - right;
                    case SyntaxType.Multiply:
                        return left * right;
                    case SyntaxType.Divide:
                        return left / right;
                    case SyntaxType.Power:
                        return (int) MathF.Pow(left, right);
                    case SyntaxType.Modulo:
                        return left % right;
                    default:
                        throw new Exception($"unexpected binary operator {b.OperatorToken.Type}");
                }
            }

            if (node is ParenthesizedExpression p)
            {
                return EvaluateExpression(p.Expression);
            }

            throw new Exception($"unexpected node {node.Type}");
        }
    }
}