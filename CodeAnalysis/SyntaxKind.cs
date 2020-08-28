﻿namespace PacLang
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
        
        
        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,        
        ParenthesizeExpression
    }
}
