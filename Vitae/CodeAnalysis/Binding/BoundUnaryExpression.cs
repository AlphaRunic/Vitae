using System;

namespace Vitae.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperatorType operatorType, BoundExpression operand)
        {
            OperatorType = operatorType;
            Operand = operand;
        }

        public override Type Type => Operand.Type;
        public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;
        public BoundUnaryOperatorType OperatorType { get; }
        public BoundExpression Operand { get; }
    }
}