using System.Collections.Generic;

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

    public override IEnumerable<SyntaxNode> GetChildren()
    {
      yield return IdentifierToken;
    }
  }
}
