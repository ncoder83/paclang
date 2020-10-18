using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PacLang.Binding;
using PacLang.CodeAnalysis.Syntax;

namespace PacLang
{

    public class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables) 
        {
            var globalScope = Binder.BindGlobalScopre(Syntax.Root);            

            var diagnostics = Syntax.Diagnostics.Concat(globalScope.Diagnostics).ToImmutableArray();

            if (diagnostics.Any())            
                return new EvaluationResult(diagnostics, null);
            
            
            var evaluator = new Evaluator(globalScope.Expression, variables);
            var value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}
