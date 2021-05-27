namespace PacLang.Binding
{
    internal sealed class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundConditionalGotoStatement(BoundLabel label, BoundExpression condition, bool jumpIfTrue = true)
        {
            Label = label;
            Condition = condition;
            JumpIfTrue = jumpIfTrue;
        }

        public BoundLabel Label { get; set; }
        public BoundExpression Condition { get; set; }
        public bool JumpIfTrue { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
    }
}