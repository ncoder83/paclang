using System;
using System.Collections.Generic;
using PacLang.Binding;

namespace PacLang
{
    /// <summary>
    /// this class will evaluate your expression
    /// </summary>
    internal sealed class Evaluator
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;
        private object _lastValue;

        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate()
        {
            EvaluateStatement(_root);
            return _lastValue;
        }

        private void EvaluateStatement(BoundStatement node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.BlockStatement:
                    EvaluateBlockStatement((BoundBlockStatement)node);
                    break;
                case BoundNodeKind.VariableDeclaration:
                    EvaluateVariableDeclaration((BoundVariableDeclaration)node);
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EvaluateExpressionStatement((BoundExpressionStatement)node);
                    break;
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            };
        }

        private void EvaluateVariableDeclaration(BoundVariableDeclaration node)
        {
            var value = EvaluateExpression(node.Initializer);
            _variables[node.Variable] = value;
            _lastValue = value;
        }

        private void EvaluateBlockStatement(BoundBlockStatement node)
        {
            foreach (var statement in node.Statements)
                EvaluateStatement(statement);

        }

        private void EvaluateExpressionStatement(BoundExpressionStatement statement)
        {
            _lastValue = EvaluateExpression(statement.Expression);
        }



        private object EvaluateExpression(BoundExpression expression)
        {
            return expression.Kind switch
            {
                BoundNodeKind.LiteralExpression => EvaluateLiteralExpression((BoundLiteralExpression)expression),
                BoundNodeKind.VariableExpression => EvaluateVariableExpression((BoundVariableExpression)expression),
                BoundNodeKind.AssigmentExpression => EvaluateAssigmentExpression((BoundAssigmentExpression)expression),
                BoundNodeKind.UnaryExpression => EvaluateUnaryExpression((BoundUnaryExpression)expression),
                BoundNodeKind.BinaryExpression => EvaluateBinaryExpression((BoundBinaryExpression)expression),
                _ => throw new Exception($"Unexpected node {expression.Kind}"),
            };
        }


        private object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }
        private object EvaluateVariableExpression(BoundVariableExpression v)
        {
            return _variables[v.Variable];
        }

        private object EvaluateAssigmentExpression(BoundAssigmentExpression a)
        {
            var value = EvaluateExpression(a.Expression);
            _variables[a.Variable] = value;
            return value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
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

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
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

                BoundBinaryOperatorKind.Less => (int) left < (int) right,
                BoundBinaryOperatorKind.LessOrEquals => (int)left <= (int)right,
                BoundBinaryOperatorKind.Greater => (int)left > (int)right,
                BoundBinaryOperatorKind.GreaterOrEquals => (int)left >= (int)right,
                _ => throw new Exception($"Unexpected binary behavior {b.Op}"),
            };
        }
    }
}
