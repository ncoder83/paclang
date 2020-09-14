using System;

namespace PacLang.Binding
{
    internal sealed class BoundAssigmentExpression : BoundExpression
    {


        public BoundAssigmentExpression(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }

        public override Type Type => Expression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.AssigmentExpression;
    }
}