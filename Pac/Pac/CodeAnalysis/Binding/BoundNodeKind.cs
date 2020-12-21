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
        LiteralExpression,
        VariableExpression,
        AssigmentExpression,
        UnaryExpression,
        BinaryExpression,
        
    }
}