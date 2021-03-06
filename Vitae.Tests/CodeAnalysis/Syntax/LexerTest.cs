using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

using Vitae.CodeAnalysis.Syntax;


namespace Vitae.Tests.CodeAnalysis.Syntax
{
    public class LexerTest
    {
        [Fact]
        public void Lexer_Tests_AllTokens()
        {
            var tokenTypes = Enum.GetValues(typeof(SyntaxType))
                                .Cast<SyntaxType>()
                                .Where(t => t.ToString().EndsWith("Keyword") ||
                                            t.ToString().EndsWith("Token"));
            
            var testedTokenTypes = GetTokens().Concat(GetSeparators()).Select(t => t.type);
            var untestedTokenTypes = new SortedSet<SyntaxType>(tokenTypes);
            untestedTokenTypes.Remove(SyntaxType.InvalidToken);
            untestedTokenTypes.Remove(SyntaxType.EOFToken);
            untestedTokenTypes.ExceptWith(testedTokenTypes);

            Assert.Empty(untestedTokenTypes);
        }

        [Theory]
        [MemberData(nameof(GetTokensData))]
        public void Lexer_Lexes_Token(SyntaxType type, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);

            var token = Assert.Single(tokens);
            Assert.Equal(type, token.Type);
            Assert.Equal(text, token.Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Lexes_TokenPairs(SyntaxType t1Type, string t1Text, SyntaxType t2Type, string t2Text)
        {
            var text = t1Text + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(2, tokens.Length);
            Assert.Equal(tokens[0].Type, t1Type);
            Assert.Equal(tokens[0].Text, t1Text);
            Assert.Equal(tokens[1].Type, t2Type);
            Assert.Equal(tokens[1].Text, t2Text);
        }

        [Theory]
        [MemberData(nameof(GetTokenPairsWithSeparatorData))]
        public void Lexer_Lexes_TokenPairs_WithSeparators(SyntaxType t1Type, string t1Text, SyntaxType separatorType, string separatorText, SyntaxType t2Type, string t2Text)
        {
            var text = t1Text + separatorText + t2Text;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.Equal(3, tokens.Length);
            Assert.Equal(tokens[0].Type, t1Type);
            Assert.Equal(tokens[0].Text, t1Text);
            Assert.Equal(tokens[1].Type, separatorType);
            Assert.Equal(tokens[1].Text, separatorText);
            Assert.Equal(tokens[2].Type, t2Type);
            Assert.Equal(tokens[2].Text, t2Text);
        }

        public static IEnumerable<object[]> GetTokensData()
        {
            foreach (var t in GetTokens().Concat(GetSeparators()))
                yield return new object[] { t.type, t.text };
        }

        public static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var t in GetTokenPairs())
                yield return new object[] { t.t1Type, t.t1Text, t.t2Type, t.t2Text };
        }

        public static IEnumerable<object[]> GetTokenPairsWithSeparatorData()
        {
            foreach (var t in GetTokenPairsWithSeparator())
                yield return new object[] { t.t1Type, t.t1Text, t.separatorType, t.separatorText, t.t2Type, t.t2Text };
        }

        private static IEnumerable<(SyntaxType type, string text)> GetTokens()
        {
            var fixedTokens = Enum.GetValues(typeof(SyntaxType))
                                    .Cast<SyntaxType>()
                                    .Select(t => (type: t, text: SyntaxFacts.GetText(t)))
                                    .Where(t => t.text != null);

            var dynamicTokens = new[]
            {
                (SyntaxType.NumberToken, "1"),
                (SyntaxType.NumberToken, "123"),
                (SyntaxType.IdentifierToken, "a"),
                (SyntaxType.IdentifierToken, "abc"),
            };

            return fixedTokens.Concat(dynamicTokens);
        }

        private static IEnumerable<(SyntaxType type, string text)> GetSeparators()
        {
            return new[]
            {
                (SyntaxType.WhitespaceToken, " "),
                (SyntaxType.WhitespaceToken, "  "),
                (SyntaxType.WhitespaceToken, "\r"),
                (SyntaxType.WhitespaceToken, "\n"),
                (SyntaxType.WhitespaceToken, "\r\n")
            };
        }

        private static bool RequiresSeparator(SyntaxType t1Type, SyntaxType t2Type)
        {
            var t1IsKeyword = t1Type.ToString().EndsWith("Keyword");
            var t2IsKeyword = t2Type.ToString().EndsWith("Keyword");

            if (t1Type == SyntaxType.IdentifierToken && t2Type == SyntaxType.IdentifierToken)
                return true;

            if (t1IsKeyword && t2IsKeyword)
                return true;

            if (t1IsKeyword && t2Type == SyntaxType.IdentifierToken)
                return true;

            if (t1Type == SyntaxType.IdentifierToken && t2IsKeyword)
                return true;

            if (t1Type == SyntaxType.NumberToken && t2Type == SyntaxType.NumberToken)
                return true;

            if (t1Type == SyntaxType.BangToken && t2Type == SyntaxType.EqualsToken)
                return true;

            if (t1Type == SyntaxType.BangToken && t2Type == SyntaxType.EqualToToken)
                return true;

            if (t1Type == SyntaxType.EqualsToken && t2Type == SyntaxType.EqualsToken)
                return true;

            if (t1Type == SyntaxType.EqualsToken && t2Type == SyntaxType.EqualToToken)
                return true;

            return false;
        }

        private static IEnumerable<(SyntaxType t1Type, string t1Text, SyntaxType t2Type, string t2Text)> GetTokenPairs()
        {
            foreach (var t1 in  GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (!RequiresSeparator(t1.type, t2.type))
                        yield return (t1.type, t1.text, t2.type, t2.text);
                }
            }
        }

        private static IEnumerable<(SyntaxType t1Type, string t1Text, SyntaxType separatorType, string separatorText, SyntaxType t2Type, string t2Text)> GetTokenPairsWithSeparator()
        {
            foreach (var t1 in GetTokens())
            {
                foreach (var t2 in GetTokens())
                {
                    if (RequiresSeparator(t1.type, t2.type))
                    {
                        foreach (var s in GetSeparators())
                            yield return (t1.type, t1.text, s.type, s.text, t2.type, t2.text);
                    }
                }
            }
        }
    }
}