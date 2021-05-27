namespace PacLang.Binding
{
    internal sealed class BoundGotoStatement : BoundStatement
    {
        public BoundGotoStatement(BoundLabel label)
        {
            Label = label;
        }

        public BoundLabel Label { get; set; }
        public override BoundNodeKind Kind => BoundNodeKind.GotoStatement;
    }
}