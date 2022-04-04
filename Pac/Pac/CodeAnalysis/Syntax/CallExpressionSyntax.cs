﻿namespace PacLang.CodeAnalysis.Syntax
{

    public sealed class CallExpressionSyntax : ExpressionSyntax
    {

        public CallExpressionSyntax(SyntaxToken identifier, SyntaxToken openParenthesis ,SeparatedSyntaxList<ExpressionSyntax> arguments, SyntaxToken closeParenthesis)
        {
            Identifier = identifier;
            OpenParenthesis = openParenthesis;
            Arguments = arguments;
            CloseParenthesis = closeParenthesis;
        }

        public override SyntaxKind Kind => SyntaxKind.CallExpression;
        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenParenthesis { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public SyntaxToken CloseParenthesis { get; }
    }
}
