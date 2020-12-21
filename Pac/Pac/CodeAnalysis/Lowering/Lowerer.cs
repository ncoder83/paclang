
using PacLang.Binding;
using PacLang.CodeAnalysis.Syntax;
using System.Collections.Immutable;

namespace Pac.CodeAnalysis.Lowering
{
    internal sealed class Lowerer : BoundTreeRewriter
    {
        private Lowerer() { }

        public static BoundStatement Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            return lowerer.RewriteStatement(statement);
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {

            /*            
                for <var> = <lower> to <upper>
                    <body>

                ----->                
                {
                    var <var> = <lower>
                    while (<var> <= <upper>)
                    {
                        <body>
                        <var> = <var> + 1
                    }
                }
             */
            // var <var> = <lower>
            var variableDeclaration = new BoundVariableDeclaration(node.Variable, node.LowerBound);

            // <var>
            var variableExpression = new BoundVariableExpression(node.Variable);

            //<var> <= <upper>
            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LessOrEqualsToken, typeof(int), typeof(int)),
                node.UpperBound
            );

            // <var> = <var> + 1
            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(
                    node.Variable,
                    new BoundBinaryExpression(
                        variableExpression,
                        BoundBinaryOperator.Bind(SyntaxKind.PlusToken, typeof(int), typeof(int)),
                        new BoundLiteralExpression(1)
                    )
                )
            );

            var whileBlock = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(node.Body, increment));
            var whileStatement = new BoundWhileStatement(condition, whileBlock);
            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(variableDeclaration, whileStatement));
            return RewriteStatement(result);
        }
    }
}
