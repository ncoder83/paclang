namespace PacLang.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,

        // Expressions
        LiteralExpression,
        VariableExpression,
        AssigmentExpression,
        UnaryExpression,
        BinaryExpression,        
    }
}