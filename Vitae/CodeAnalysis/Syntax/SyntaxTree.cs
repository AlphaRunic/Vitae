using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(ImmutableArray<Diagnostic> diagnostics, ExpressionSyntax root, Token EOF)
        {
            Diagnostics = diagnostics;
            Root = root;
            this.EOF = EOF;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public Token EOF { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<Token> ParseTokens(string text)
        {
            Lexer lexer = new Lexer(text);
            
            while (true)
            {
                Token token = lexer.Lex();
                if (token.Type == SyntaxType.EOFToken)
                    break;
                
                yield return token;
            }
        }
    }
}