using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _pos;
        private List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current => Peek(0);
        private char LookAhead => Peek(1);

        private char Peek(int offset)
        {
            int index = _pos + offset;

            if (index >= _text.Length)
                return '\0';

            return _text[_pos];
        }

        private void Next() {
            _pos++;
        }

        public Token Lex() {
            if (_pos >= _text.Length) {
                return new Token(SyntaxType.EOF, _pos, "\0", null);
            }

            if (char.IsDigit(Current)) {
                var start = _pos;

                while(char.IsDigit(Current))
                    Next();

                var length = _pos - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"Error: the number {_text} isn't a valid Int32");

                return new Token(SyntaxType.Number, start, text, value);
            }

            if (char.IsWhiteSpace(Current)) {
                var start = _pos;

                while(char.IsWhiteSpace(Current))
                    Next();

                var length = _pos - start;
                var text = _text.Substring(start, length);
                return new Token(SyntaxType.Whitespace, start, text, null);
            }

            if (char.IsLetter(Current))
            {
                var start = _pos;

                while(char.IsLetter(Current))
                    Next();

                var length = _pos - start;
                var text = _text.Substring(start, length);
                var type = SyntaxFacts.GetKeywordType(text);
                return new Token(type, start, text, null);
            }

            switch(Current) {
                case '+':
                    return new Token(SyntaxType.Plus, _pos++, "+", null);
                case '-':
                    return new Token(SyntaxType.Minus, _pos++, "-", null);
                case '*':
                    return new Token(SyntaxType.Multiply, _pos++, "*", null);
                case '/':
                    return new Token(SyntaxType.Divide, _pos++, "/", null);
                case '%':
                    return new Token(SyntaxType.Modulo, _pos++, "%", null);
                case '^':
                    return new Token(SyntaxType.Power, _pos++, "^", null);
                case '(':
                    return new Token(SyntaxType.OpenParen, _pos++, "(", null);
                case ')':
                    return new Token(SyntaxType.ClosedParen, _pos++, ")", null);

                case '!':
                    return new Token(SyntaxType.Bang, _pos++, "!", null);
                case '&':
                    return new Token(SyntaxType.Ampersand, _pos++ , "&", null);
                case '|':
                    return new Token(SyntaxType.Pipe, _pos++ , "|", null);


                default:
                    _diagnostics.Add($"Error: bad character input: {Current}");
                    return new Token(SyntaxType.Invalid, _pos++, _text.Substring(_pos - 1, 1), null);  
            }
        }
    }
}
