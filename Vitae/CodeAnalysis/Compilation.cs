using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Vitae.CodeAnalysis.Binding;
using Vitae.CodeAnalysis.Syntax;

namespace Vitae.CodeAnalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree tree)
        {
            Tree = tree;
        }

        public SyntaxTree Tree { get; }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(Tree.Root.Expression);

            var diagnostics = Tree.Diagnostics.Concat(binder.Diagnostics).ToImmutableArray();
            if (diagnostics.Any())
                return new EvaluationResult(diagnostics, null);

            var eval = new Evaluator(boundExpression, variables);
            var value = eval.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}