﻿
using PacLang;
using PacLang.Binding;
using PacLang.CodeAnalysis.Syntax;
using PacLang.Symbols;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Pac.CodeAnalysis.Lowering
{
    internal sealed class Lowerer : BoundTreeRewriter
    {
        private int _labelCount;
        private Lowerer() { }

        private BoundLabel GenerateLabel()
        {
            var name = $"Label{++_labelCount}";
            return new BoundLabel(name);
        }

        public static BoundBlockStatement Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            var result =  lowerer.RewriteStatement(statement);
            return Flatten(result);
        }

        private static BoundBlockStatement Flatten(BoundStatement statement) 
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var stack = new Stack<BoundStatement>();
            stack.Push(statement);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if(current is BoundBlockStatement block) 
                {
                    foreach (var s in block.Statements.Reverse())
                        stack.Push(s);                    
                }
                else
                {
                    builder.Add(current);
                }
            }
            return new BoundBlockStatement(builder.ToImmutable());
        }

        protected override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            if (node.ElseStatement == null)
            {
                //if <condition> 
                //    <then>                
                //
                //------->
                //
                //gotoFalse <condition> end
                //<then>
                //end:
                var endLabel = GenerateLabel();
                var gotoFalse = new BoundConditionalGotoStatement(endLabel, node.Condition, false);
                var endLabelStatement = new BoundLabelStatement(endLabel);
                var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                    gotoFalse, 
                    node.ThenStatement, 
                    endLabelStatement
                ));
                return RewriteStatement(result);
            }
            else
            {
                //if <condition> 
                //    <then>                
                //else 
                //    <then>
                //
                //------->
                //
                //gotoFalse <condition> else
                //<then>
                //goto end
                //else:
                //<else>
                //end:
                var elseLabel = GenerateLabel();
                var endLabel = GenerateLabel();
                
                var gotoFalse = new BoundConditionalGotoStatement(elseLabel, node.Condition, false);
                var gotoEndStatement = new BoundGotoStatement(endLabel);
                var elseLabelStatement = new BoundLabelStatement(elseLabel);
                var endLabelStatement = new BoundLabelStatement(endLabel);

                var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                    gotoFalse, 
                    node.ThenStatement, 
                    gotoEndStatement,
                    elseLabelStatement,
                    node.ElseStatement,
                    endLabelStatement
                ));
                return RewriteStatement(result);

            }

        }

        protected override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            // while <condition>
            //     <body>
            //
            // ------->
            // goto check
            // continue:
            // <body>
            // check:
            // gotoTrue <condition> continue
            // end:
            var endLabel = GenerateLabel();
            var checkLabel = GenerateLabel();
            var continueLabel = GenerateLabel();

            var gotoCheck = new BoundGotoStatement(checkLabel);
            var continueLabelStatement = new BoundLabelStatement(continueLabel);
            var checkLabelStatement = new BoundLabelStatement(checkLabel);
            var gotoTrue = new BoundConditionalGotoStatement(continueLabel, node.Condition);
            var endLabelStatement = new BoundLabelStatement(endLabel);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                   gotoCheck,
                   continueLabelStatement,
                   node.Body,
                   checkLabelStatement,
                   gotoTrue,
                   endLabelStatement
               ));
            return RewriteStatement(result);
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {

            /*            
                for <var> = <lower> to <upper>
                    <body>

                ----->                
                {
                    var <var> = <lower>
                    let upperBound = <upper>
                    while (<var> <= <upperBound>)
                    {
                        <body>
                        <var> = <var> + 1
                    }
                }
             */
            // var <var> = <lower>
            var variableDeclaration = new BoundVariableDeclaration(node.Variable, node.LowerBound);

            // let upperBound = <upper>
            var upperBoundSymbol = new VariableSymbol("upperBound", true, TypeSymbol.Int);
            var upperBoundDeclaration = new BoundVariableDeclaration(upperBoundSymbol, node.UpperBound);

            // <var>
            var variableExpression = new BoundVariableExpression(node.Variable);

            //<var> <= <upperBound>
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LessOrEqualsToken, TypeSymbol.Int, TypeSymbol.Int),
                new BoundVariableExpression(upperBoundSymbol)
            );

            // <var> = <var> + 1
            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(
                    node.Variable,
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(SyntaxKind.PlusToken, TypeSymbol.Int, TypeSymbol.Int),
                        new BoundLiteralExpression(1)
                    )
                )
            );

            var whileBlock = new BoundBlockStatement(ImmutableArray.Create(node.Body, increment));
            var whileStatement = new BoundWhileStatement(condition, whileBlock);

            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(
                variableDeclaration, 
                upperBoundDeclaration,
                whileStatement));
            return RewriteStatement(result);
        }
    }
}
