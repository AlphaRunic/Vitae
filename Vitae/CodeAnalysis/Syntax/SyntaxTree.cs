using System.Collections.Generic;
using System.Linq;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(DiagnosticBag diagnostics, ExpressionSyntax root, Token EOF)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            this.EOF = EOF;
        }

        public Diagnostic[] Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public Token EOF { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}