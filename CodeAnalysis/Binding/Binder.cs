using System;

namespace PacLang.Binding
{

    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get; }
    }

    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type{ get;}
        public override BoundNodeKind Kind => throw new System.NotImplementedException();
    }

    internal sealed class BoundUnaryExpression : BoundExpression
    {
       
        public override BoundNodeKind Kind => throw new System.NotImplementedException();

        public override Type Type => throw new NotImplementedException();

        public Binder(Parameters)
        {
            
        }
    }

}