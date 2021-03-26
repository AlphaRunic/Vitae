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
            new BoundBinaryOperator(SyntaxType.PlusToken, BoundBinaryOperatorType.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxType.MinusToken, BoundBinaryOperatorType.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxType.StarToken, BoundBinaryOperatorType.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxType.SlashToken, BoundBinaryOperatorType.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxType.CaratToken, BoundBinaryOperatorType.Exponentation, typeof(int)),
            new BoundBinaryOperator(SyntaxType.PercentToken, BoundBinaryOperatorType.Modulus, typeof(int)),

            new BoundBinaryOperator(SyntaxType.EqualToToken, BoundBinaryOperatorType.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.NotEqualToToken, BoundBinaryOperatorType.NotEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxType.EqualToToken, BoundBinaryOperatorType.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.NotEqualToToken, BoundBinaryOperatorType.NotEquals, typeof(bool)),

            new BoundBinaryOperator(SyntaxType.AmpersandToken, BoundBinaryOperatorType.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxType.PipeToken, BoundBinaryOperatorType.LogicalOr, typeof(bool))
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