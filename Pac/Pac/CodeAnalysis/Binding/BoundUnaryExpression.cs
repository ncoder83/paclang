using PacLang.Symbols;
using System;

namespace PacLang.Binding
{

    internal sealed class BoundUnaryExpression : BoundExpression
    {               
        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand)
        {
            Op = op;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override TypeSymbol Type => Op.ResultType;
        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }
    }

}