﻿using System;

namespace PacLang.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type{ get;}        
    }

}