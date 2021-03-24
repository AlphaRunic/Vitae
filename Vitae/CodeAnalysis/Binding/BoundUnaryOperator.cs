using System;
using Vitae.CodeAnalysis.Syntax;

namespace Vitae.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType type, Type operandType)
            : this(syntaxType, type, operandType, operandType)
        {
        }

        private BoundUnaryOperator(SyntaxType syntaxType, BoundUnaryOperatorType type, Type operandType, Type resultType)
        {
            SyntaxType = syntaxType;
            Type = type;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxType SyntaxType { get; }
        public BoundUnaryOperatorType Type { get; }
        public Type OperandType { get; }
        public Type ResultType { get; } 

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxType.Bang, BoundUnaryOperatorType.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxType.Plus, BoundUnaryOperatorType.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxType.Minus, BoundUnaryOperatorType.Negation, typeof(int)),
        };

        public static BoundUnaryOperator Bind(SyntaxType syntaxType, Type operandType)
        {
            foreach (var op in _operators)
            {
                if (op.SyntaxType == syntaxType && op.OperandType == operandType)
                {
                    return op;
                }
            }

            return null;
        }
    }
}