using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Vitae.CodeAnalysis.Text;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(SourceText text, ImmutableArray<Diagnostic> diagnostics, ExpressionSyntax root, Token EOF)
        {
            Text = text;
            Diagnostics = diagnostics;
            Root = root;
            this.EOF = EOF;
        }

        public SourceText Text { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public Token EOF { get; }

        public static SyntaxTree Parse(string text)
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }

        public static SyntaxTree Parse(SourceText text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<Token> ParseTokens(string text)
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }

        public static IEnumerable<Token> ParseTokens(SourceText text)
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