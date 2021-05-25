namespace PacLang.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        VariableDeclaration,
        IfStatement,
        ExpressionStatement,
        WhileStatement,
        ForStatement,
        GotoStatement,
        ConditionalGotoStatement,
        LabelStatement,


        // Expressions
        ErrorExpression,
        LiteralExpression,
        VariableExpression,
        AssigmentExpression,
        UnaryExpression,
        BinaryExpression,        
    }
}