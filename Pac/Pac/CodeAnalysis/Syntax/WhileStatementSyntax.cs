using PacLang.CodeAnalysis.Syntax;

namespace Pac.CodeAnalysis.Syntax
{
    public sealed class WhileStatementSyntax : StatementSyntax
    {

        public WhileStatementSyntax(SyntaxToken token, ExpressionSyntax condition, StatementSyntax body)
        {
            Token = token;
            Condition = condition;
            Body = body;
        }
        public override SyntaxKind Kind => SyntaxKind.WhileStatement;

        public SyntaxToken Token { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax Body { get; }
    }
}
