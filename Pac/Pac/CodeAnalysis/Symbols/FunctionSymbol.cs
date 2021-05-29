using System.Collections.Immutable;

namespace PacLang.Symbols
{



    public sealed class FunctionSymbol : Symbol
    {
        public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameter, TypeSymbol type) 
            : base(name)
        {
            Parameter = parameter;
            Type = type;
        }
        public override SymbolKind Kind => SymbolKind.Function;
        public ImmutableArray<ParameterSymbol> Parameter { get; set; }
        public TypeSymbol Type { get; }
    }
}
