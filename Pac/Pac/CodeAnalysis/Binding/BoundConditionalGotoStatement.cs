namespace PacLang.Binding
{
    internal sealed class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundConditionalGotoStatement(LabelSymbol label, BoundExpression condition, bool jumpIfFalse = false)
        {
            Label = label;
            Condition = condition;
            JumpIfFalse = jumpIfFalse;
        }

        public LabelSymbol Label { get; set; }
        public BoundExpression Condition { get; set; }
        public bool JumpIfFalse { get; set; }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
    }
}