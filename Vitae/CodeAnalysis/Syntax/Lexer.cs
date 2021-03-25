using System;
using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly string _text;

        private int _pos;
        private int _start;
        private SyntaxType _type;
        private object _value;

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

            return _text[index];
        }

        private void Next() {
            _pos++;
        }

        public Token Lex() {
            _start = _pos;
            _type = SyntaxType.Invalid;
            _value = null;
            
            switch(Current) {
                    case '\0':
                        _type = SyntaxType.EOF;
                        break;
                    case '+':
                        _type = SyntaxType.Plus;
                        _pos++;
                        break;
                    case '-':
                        _type = SyntaxType.Minus;
                        _pos++;
                        break;
                    case '*':
                        _type = SyntaxType.Multiply;
                        _pos++;
                        break;
                    case '/':
                        _type = SyntaxType.Divide;
                        _pos++;
                        break;
                    case '%':
                        _type = SyntaxType.Modulo;
                        _pos++;
                        break;
                    case '^':
                        _type = SyntaxType.Power;
                        _pos++;
                        break;
                    case '(':
                        _type = SyntaxType.OpenParen;
                        _pos++;
                        break;
                    case ')':
                        _type = SyntaxType.ClosedParen;
                        _pos++;
                        break;

                    case '&':
                        _type = SyntaxType.Ampersand;
                        _pos++;
                        break;
                    case '|':
                        _type = SyntaxType.Pipe;
                        _pos++;
                        break;
                    case '=':
                    {
                        _pos++;
                        if (Current != '=')
                            _type = SyntaxType.Assignment;
                        else
                        {
                            _pos++;
                            _type = SyntaxType.EqualTo; 
                        }
                        break;
                    }
                    case '!':
                    {
                        _pos++;
                        if (Current != '=')
                            _type = SyntaxType.Bang;
                        else
                        {
                            _pos++;
                            _type = SyntaxType.NotEqualTo;
                        }
                        break;
                    } 

                    case '0': case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
                        ReadNumber();
                        break;

                    case ' ': case '\t': case '\n': case '\r':  
                        ReadWhitespace();
                        break;

                    default:
                        if (char.IsLetter(Current))
                        {
                            ReadIdentifierOrKeyword();
                        }
                        else if (char.IsWhiteSpace(Current))
                        {
                            ReadWhitespace();
                        }
                        else
                        {
                            _diagnostics.ReportInvalidCharacter(_pos, Current);
                            _pos++;
                        }
                        break;
                }

            string text = SyntaxFacts.GetText(_type);
            int length = _pos - _start;
            if (text == null)
                text = _text.Substring(_start, length);

            return new Token(_type, _start, text, _value);
        }

        private void ReadWhitespace()
        {
            while (char.IsWhiteSpace(Current))
                _pos++;

            _type = SyntaxType.Whitespace;
        }

        private void ReadNumber()
        {
            while (char.IsDigit(Current))
                _pos++;

            var length = _pos - _start;
            var text = _text.Substring(_start, length);
            if (!int.TryParse(text, out var value))
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), _text, typeof(int));

            _value = value;
            _type = SyntaxType.Number;
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
                _pos++;

            var length = _pos - _start;
            var text = _text.Substring(_start, length);
            _type = SyntaxFacts.GetKeywordType(text);
        }
    }
}
