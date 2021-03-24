using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _pos;
        private DiagnosticBag _diagnostics = new DiagnosticBag();

        public Lexer(string text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

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
            var start = _pos;
            
            if (_pos >= _text.Length) {
                return new Token(SyntaxType.EOF, _pos, "\0", null);
            }

            if (char.IsDigit(Current)) {
                while(char.IsDigit(Current))
                    Next();

                var length = _pos - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.ReportInvalidNumber(new TextSpan(start, length), _text, typeof(int));

                return new Token(SyntaxType.Number, start, text, value);
            }

            if (char.IsWhiteSpace(Current)) {
                while(char.IsWhiteSpace(Current))
                    Next();

                var length = _pos - start;
                var text = _text.Substring(start, length);
                return new Token(SyntaxType.Whitespace, start, text, null);
            }

            if (char.IsLetter(Current))
            {
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

                case '&':
                    return new Token(SyntaxType.Ampersand, _pos++ , "&", null);
                case '|':
                    return new Token(SyntaxType.Pipe, _pos++ , "|", null);
                case '=':
                {
                    if (LookAhead == '=')
                    {
                        _pos += 2;
                        return new Token(SyntaxType.EqualTo, start, "==", null);
                    }
                    break;
                }
                case '!':
                {
                    if (LookAhead == '=')
                    {
                        _pos += 2;
                        return new Token(SyntaxType.NotEqualTo, start, "!=", null);
                    }
                    else
                    {
                        _pos += 1;
                        return new Token(SyntaxType.Bang, start, "!", null);
                    }
                }                    
            }

            _diagnostics.ReportInvalidCharacter(_pos, Current);
            return new Token(SyntaxType.Invalid, _pos++, _text.Substring(_pos - 1, 1), null);  
        }
    }
}
