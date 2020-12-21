namespace PacLang.Binding
{
    internal sealed class BoundLabelStatement : BoundStatement 
    {
        public BoundLabelStatement(LabelSymbol label)
        {
            Label = label;
        }

        public LabelSymbol Label { get; set; }
        public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
    }
}