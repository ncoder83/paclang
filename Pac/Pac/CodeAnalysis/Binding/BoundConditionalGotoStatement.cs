namespace PacLang.Binding
{
    internal sealed class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundConditionalGotoStatement(LabelSymbol label, BoundExpression condition, bool jumpIfTrue = true)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }

        public LabelSymbol Label { get; set; }
        public BoundExpression Condition { get; set; }
        public bool JumpIfTrue { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
    }
}