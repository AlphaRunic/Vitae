using System;
using Vitae.CodeAnalysis.Syntax;

namespace Vitae.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType type, Type nodeType)
            : this(syntaxType, type, nodeType, nodeType, nodeType)
        {
        }

        private BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType type, Type operandType, Type resultType)
            : this(syntaxType, type, operandType, operandType, resultType)
        {
        }

        private BoundBinaryOperator(SyntaxType syntaxType, BoundBinaryOperatorType type, Type leftType, Type rightType, Type resultType)
        {
            SyntaxType = syntaxType;
            Type = type;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        public SyntaxType SyntaxType { get; }
        public BoundBinaryOperatorType Type { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxType.Plus, BoundBinaryOperatorType.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxType.Minus, BoundBinaryOperatorType.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxType.Multiply, BoundBinaryOperatorType.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxType.Divide, BoundBinaryOperatorType.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxType.Power, BoundBinaryOperatorType.Exponentation, typeof(int)),
            new BoundBinaryOperator(SyntaxType.Modulo, BoundBinaryOperatorType.Modulus, typeof(int)),

            new BoundBinaryOperator(SyntaxType.EqualTo, BoundBinaryOperatorType.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.NotEqualTo, BoundBinaryOperatorType.NotEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.EqualTo, BoundBinaryOperatorType.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.NotEqualTo, BoundBinaryOperatorType.NotEquals, typeof(bool)),

            new BoundBinaryOperator(SyntaxType.Ampersand, BoundBinaryOperatorType.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.Pipe, BoundBinaryOperatorType.LogicalOr, typeof(bool))
        };

        public static BoundBinaryOperator Bind(SyntaxType syntaxType, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxType == syntaxType && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }

            return null;
        }
    }
}