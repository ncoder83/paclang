namespace PacLang.Symbols
{
    public abstract class Symbol
    {
        private protected Symbol(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public abstract SymbolKind Kind { get; }

    }
}
