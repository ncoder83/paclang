namespace PacLang.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhiteSpaceToken,
        NumberToken,
        StringToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        BangToken,
        EqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        TildeToken,
        AmpersandToken,
        PipeToken,
        HatToken,
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
        WhileKeyword,
        ForKeyword,
        ToKeyword,

        // Nodes
        CompilationUnit,

        // Statements
        BlockStatement,
        VariableDeclaration,
        IfStatement,
        ExpressionStatement,
        WhileStatement,
        ForStatement,

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
