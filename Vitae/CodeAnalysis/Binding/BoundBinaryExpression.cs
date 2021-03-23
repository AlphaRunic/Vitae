using System;

namespace Vitae.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperatorType operatorType, BoundExpression right)
        {
            Left = left;
            OperatorType = operatorType;
            Right = right;
        }

        public override Type Type => Left.Type;
        public override BoundNodeType NodeType => BoundNodeType.UnaryExpression;
        public BoundExpression Left { get; }
        public BoundBinaryOperatorType OperatorType { get; }
        public BoundExpression Right { get; }
    }
}