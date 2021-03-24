using System;

namespace Vitae.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.Plus:
                case SyntaxType.Minus:
                case SyntaxType.Bang:
                    return 7;
                
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.Power:
                    return 6;

                case SyntaxType.Modulo:
                case SyntaxType.Multiply:
                case SyntaxType.Divide:
                    return 5;

                case SyntaxType.Plus:
                case SyntaxType.Minus:
                    return 4;

                case SyntaxType.EqualTo:
                case SyntaxType.NotEqualTo:
                    return 3;

                case SyntaxType.Ampersand:
                    return 2;
                case SyntaxType.Pipe:
                    return 1;
                
                default:
                    return 0;
            }
        }

        public static SyntaxType GetKeywordType(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxType.True;
                case "false":
                    return SyntaxType.False;
                
                default:
                    return SyntaxType.Identifier;
            }
        }
    }
}