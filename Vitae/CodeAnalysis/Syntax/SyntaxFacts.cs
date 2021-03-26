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
                case SyntaxType.PlusToken:
                case SyntaxType.MinusToken:
                case SyntaxType.BangToken:
                    return 7;
                
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.CaratToken:
                    return 6;

                case SyntaxType.PercentToken:
                case SyntaxType.StarToken:
                case SyntaxType.SlashToken:
                    return 5;

                case SyntaxType.PlusToken:
                case SyntaxType.MinusToken:
                    return 4;

                case SyntaxType.EqualToToken:
                case SyntaxType.NotEqualToToken:
                    return 3;

                case SyntaxType.AmpersandToken:
                    return 2;
                case SyntaxType.PipeToken:
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
                    return SyntaxType.IdentifierToken;
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
                case SyntaxType.PlusToken:
                    return "+";
                case SyntaxType.MinusToken:
                    return "-";
                case SyntaxType.StarToken:
                    return "*";
                case SyntaxType.SlashToken:
                    return "/";
                case SyntaxType.CaratToken:
                    return "^";
                case SyntaxType.PercentToken:
                    return "%";
                case SyntaxType.BangToken:
                    return "!";
                case SyntaxType.EqualsToken:
                    return "=";
                case SyntaxType.AmpersandToken:
                    return "&";
                case SyntaxType.PipeToken:
                    return "|";
                case SyntaxType.EqualToToken:
                    return "==";
                case SyntaxType.NotEqualToToken:
                    return "!=";
                case SyntaxType.OpenParenToken:
                    return "(";
                case SyntaxType.ClosedParenToken:
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