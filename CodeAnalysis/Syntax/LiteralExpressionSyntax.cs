using System;
using System.Collections.Generic;

namespace PacLang.CodeAnalysis.Syntax
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public SyntaxToken NumberToken { get; }

        public LiteralExpressionSyntax(SyntaxToken literalToken)
        {
            NumberToken = literalToken;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }
    }
}
