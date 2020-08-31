﻿namespace PacLang.CodeAnalysis.Syntax
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
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,

        //Keywords
        FalseKeyword,
        TrueKeyword,        

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,        
        ParenthesizeExpression,
    }
}
