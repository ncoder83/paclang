using System;

namespace PacLang.Binding
{
  internal sealed class BoundAssigmentExpression : BoundExpression
  {
    

    public BoundAssigmentExpression(string name, BoundExpression expression)
    {
      Name = name;
      Expression = expression;
    }

    public string Name { get; }
    public BoundExpression Expression { get; }

        public override Type Type => Expression.Type;

    public override BoundNodeKind Kind => BoundNodeKind.AssigmentExpression;
  }
}