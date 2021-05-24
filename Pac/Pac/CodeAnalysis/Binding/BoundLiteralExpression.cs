using PacLang.Symbols;
using System;

namespace PacLang.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {

        public BoundLiteralExpression(object value)
        {
            Value = value;

            if (value is bool)
                Type = TypeSymbol.Bool;
            else if (value is int)
                Type = TypeSymbol.Int;
            else if (value is string)
                Type = TypeSymbol.String;
            else
                throw new Exception($"Unexpexted literal '{value}' of {value.GetType()}");


        }

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override TypeSymbol Type { get; }
        public object Value { get; }
    }

}