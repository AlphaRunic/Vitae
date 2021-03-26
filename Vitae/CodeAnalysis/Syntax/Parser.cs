using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax {
    internal sealed class Parser {
        private readonly Token[] _tokens;

        private DiagnosticBag _diagnostics = new DiagnosticBag();
        private int _pos;

        public Parser(string text) {
            var tokens = new List<Token>(); 
            var lexer = new Lexer(text);
            Token token;
            
            do {
                token = lexer.Lex();

                if (token.Type != SyntaxType.WhitespaceToken && token.Type != SyntaxType.InvalidToken) {
                    tokens.Add(token);
                }
            } while (token.Type != SyntaxType.EOFToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public DiagnosticBag Diagnostics => _diagnostics;

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

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Type, type);
            return new Token(type, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expr = ParseExpression();
            var eofToken = MatchToken(SyntaxType.EOFToken);
            return new SyntaxTree(_diagnostics, expr, eofToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseAssignmentExpression();
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {
            if (Peek(0).Type == SyntaxType.IdentifierToken && Peek(1).Type == SyntaxType.EqualsToken)
            {
                Token identifier = NextToken();
                Token operatorToken = NextToken();
                ExpressionSyntax right = ParseAssignmentExpression();
                return new AssignmentExpression(identifier, operatorToken, right);
            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression();
                left = new UnaryExpression(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Type.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence  <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpression(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Type)
            {
                case SyntaxType.OpenParenToken:
                    {
                        Token left = NextToken();
                        ExpressionSyntax expr = ParseExpression();
                        Token right = MatchToken(SyntaxType.ClosedParenToken);
                        return new ParenthesizedExpression(left, expr, right);
                    }

                case SyntaxType.FalseKeyword:
                    {
                        Token keywordToken = NextToken();
                        return new LiteralExpression(keywordToken, false);
                    }
                case SyntaxType.TrueKeyword:
                    {
                        Token keywordToken = NextToken();
                        return new LiteralExpression(keywordToken, true);
                    }

                case SyntaxType.IdentifierToken:
                {
                    Token identifier = NextToken();
                    return new NameExpression(identifier);
                }

                default:
                    {
                        Token numberToken = MatchToken(SyntaxType.NumberToken);
                        return new LiteralExpression(numberToken);
                    }
            }
        }
    }
}