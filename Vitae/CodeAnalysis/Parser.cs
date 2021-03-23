using System.Collections.Generic;

namespace Vitae.CodeAnalysis {
    public class Parser {
        private readonly Token[] _tokens;

        private List<string> _diagnostics = new List<string>();
        private int _pos;

        public Parser(string text) {
            var tokens = new List<Token>(); 

            var lexer = new Lexer(text);
            Token token;
            do {
                token = lexer.NextToken();

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

        private Token Match(SyntaxType type)
        {
            if (Current.Type == type)
                return NextToken();

            _diagnostics.Add($"Error: unexpected token <{Current.Type}>, expected <{type}>");
            return new Token(type, Current.Position, null, null);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseTerm();
        }

        public SyntaxTree Parse()
        {
            var expr = ParseTerm();
            var eofToken = Match(SyntaxType.EOF);
            return new SyntaxTree(_diagnostics, expr, eofToken);
        }

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Type == SyntaxType.Plus || Current.Type == SyntaxType.Minus)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpression(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();

            while (Current.Type == SyntaxType.Multiply || Current.Type == SyntaxType.Divide ||
                    Current.Type == SyntaxType.Power || Current.Type == SyntaxType.Modulo)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
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
                var right = Match(SyntaxType.ClosedParen);
                return new ParenthesizedExpression(left, expr, right);
            }

            var number = Match(SyntaxType.Number);
            return new NumberExpression(number);
        }
    }
}