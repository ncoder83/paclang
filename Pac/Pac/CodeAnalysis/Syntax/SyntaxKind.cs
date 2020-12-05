namespace PacLang.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhiteSpaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        BangToken,
        EqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        LessToken,
        LessOrEqualsToken,
        GreaterToken,
        GreaterOrEqualsToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        CloseBraceToken,
        OpenBraceToken,
        IdentifierToken,


        // Keywords
        IfKeyword,
        ElseKeyword,
        FalseKeyword,
        TrueKeyword,
        VarKeyword,
        LetKeyword,

        // Nodes
        CompilationUnit,

        // Statements
        BlockStatement,
        VariableDeclaration,
        IfStatement,
        ExpressionStatement,


        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,        
        ParenthesizeExpression,
        AssigmentExpression,
        ElseClause
        
    }
}
