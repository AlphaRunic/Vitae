using System.Collections.Generic;

namespace Vitae.CodeAnalysis {
    internal sealed class Parser {
        private readonly Token[] _tokens;

        private List<string> _diagnostics = new List<string>();
        private int _pos;

        public Parser(string text) {
            var tokens = new List<Token>(); 

            var lexer = new Lexer(text);
            Token token;
            do {
                token = lexer.Lex();

                if (token.Type != SyntaxType.Whitespace && token.Type != SyntaxType.Invalid) {
                    tokens.Add(token);
                }
            } while (token.Type != SyntaxType.EOF);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private Token Peek(int offset) {
            var index = _pos + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private Token Current => Peek(0);

        private Token NextToken()
        {
            var current = Current;
            _pos++;
            return current;
        }

        private Token MatchToken(SyntaxType type)
        {
            if (Current.Type == type)
                return NextToken();

            _diagnostics.Add($"Error: unexpected token <{Current.Type}>, expected <{type}>");
            return new Token(type, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expr = ParseExpression();
            var eofToken = MatchToken(SyntaxType.EOF);
            return new SyntaxTree(_diagnostics, expr, eofToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (true)
            {
                var precedence = Current.Type.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence  <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpression(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Type == SyntaxType.OpenParen)
            {
                var left = NextToken();
                var expr = ParseExpression();
                var right = MatchToken(SyntaxType.ClosedParen);
                return new ParenthesizedExpression(left, expr, right);
            }

            var number = MatchToken(SyntaxType.Number);
            return new LiteralExpression(number);
        }
    }
}