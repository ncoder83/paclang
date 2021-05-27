namespace PacLang.Binding
{
    internal sealed class BoundLabelStatement : BoundStatement 
    {
        public BoundLabelStatement(BoundLabel label)
        {
            Label = label;
        }

        public BoundLabel Label { get; set; }
        public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
    }
}