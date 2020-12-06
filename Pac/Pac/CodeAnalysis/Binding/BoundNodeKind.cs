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

        // Expressions
        LiteralExpression,
        VariableExpression,
        AssigmentExpression,
        UnaryExpression,
        BinaryExpression        
    }
}