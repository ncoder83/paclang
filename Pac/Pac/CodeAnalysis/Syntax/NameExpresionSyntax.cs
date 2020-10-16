namespace PacLang.CodeAnalysis.Syntax
{
    public sealed class NameExpresionSyntax : ExpressionSyntax
    {
        public NameExpresionSyntax(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public SyntaxToken IdentifierToken { get; }

        public override SyntaxKind Kind => SyntaxKind.NameExpression;
    }
}
