using System;
using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
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
                    return SyntaxType.TrueKeyword;
                case "false":
                    return SyntaxType.FalseKeyword;
                
                default:
                    return SyntaxType.Identifier;
            }
        }

        public static IEnumerable<SyntaxType> GetUnaryOperatorTypes()
        {
            var types = (SyntaxType[]) Enum.GetValues(typeof(SyntaxType));
            foreach (var type in types)
            {
                if (GetUnaryOperatorPrecedence(type) > 0)
                    yield return type;
            }
        }

        public static IEnumerable<SyntaxType> GetBinaryOperatorTypes()
        {
            var types = (SyntaxType[]) Enum.GetValues(typeof(SyntaxType));
            foreach (var type in types)
            {
                if (GetBinaryOperatorPrecedence(type) > 0)
                    yield return type;
            }
        }

        public static string GetText(SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.Plus:
                    return "+";
                case SyntaxType.Minus:
                    return "-";
                case SyntaxType.Multiply:
                    return "*";
                case SyntaxType.Divide:
                    return "/";
                case SyntaxType.Power:
                    return "^";
                case SyntaxType.Modulo:
                    return "%";
                case SyntaxType.Bang:
                    return "!";
                case SyntaxType.Assignment:
                    return "=";
                case SyntaxType.Ampersand:
                    return "&";
                case SyntaxType.Pipe:
                    return "|";
                case SyntaxType.EqualTo:
                    return "==";
                case SyntaxType.NotEqualTo:
                    return "!=";
                case SyntaxType.OpenParen:
                    return "(";
                case SyntaxType.ClosedParen:
                    return ")";
                case SyntaxType.FalseKeyword:
                    return "false";
                case SyntaxType.TrueKeyword:
                    return "true";

                default:
                    return null;
            }
        }
    }
}