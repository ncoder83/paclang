using PacLang.Symbols;
using System;

namespace PacLang.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract TypeSymbol Type{ get;}        
    }
}