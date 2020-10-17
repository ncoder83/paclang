namespace PacLang.CodeAnalysis.Syntax
{
    public sealed class AssignmentExpresionSyntax : ExpressionSyntax
  {
    public AssignmentExpresionSyntax(SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionSyntax expression)
    {
      IdentifierToken = identifierToken;
      EqualsToken = equalsToken;
      Expression = expression;
    }

    public SyntaxToken IdentifierToken { get; }
    public SyntaxToken EqualsToken { get; }
    public ExpressionSyntax Expression { get; }

    public override SyntaxKind Kind => SyntaxKind.AssigmentExpression;

  }
}
