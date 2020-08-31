using System;
using System.Linq.Expressions;
using PacLang.Binding;
using PacLang.CodeAnalysis.Syntax;

namespace PacLang
{
    internal sealed class Evaluator 
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate() 
        {
            return EvaluateExpression(_root);
        }


        private object EvaluateExpression(BoundExpression node)
        {
            if(node is BoundLiteralExpression n)
                return n.Value;


            if(node is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);


                return u.Op.Kind switch
                {
                    BoundUnaryOperatorKind.Identity => (int)operand,
                    BoundUnaryOperatorKind.Negation => -(int)operand,
                    BoundUnaryOperatorKind.LogicalNegation => !(bool)operand,
                    _ => throw new Exception($"Unexpected unary behavior {u.Op}"),
                };
            }

            if (node is BoundBinaryExpression b) 
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                return b.Op.Kind switch
                {
                    BoundBinaryOperatorKind.Addition => (int)left + (int)right,
                    BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
                    BoundBinaryOperatorKind.Mulitplication => (int)left * (int)right,
                    BoundBinaryOperatorKind.Division => (int)left / (int)right,
                    BoundBinaryOperatorKind.LogicalAnd => (bool)left && (bool)right,
                    BoundBinaryOperatorKind.LogicalOr => (bool)left || (bool)right,
                    BoundBinaryOperatorKind.Equals => Equals(left, right),
                    BoundBinaryOperatorKind.NotEquals => !Equals(left, right),
                    _ => throw new Exception($"Unexpected binary behavior {b.Op}"),
                };
            }
         
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}
