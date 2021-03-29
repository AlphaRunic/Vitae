using System;
using System.Collections.Generic;
using Vitae.CodeAnalysis.Syntax;
using Xunit;

namespace Vitae.Tests.CodeAnalysis.Syntax
{
    public class SyntaxFactTest
    {
        [Theory]
        [MemberData(nameof(GetSyntaxTypeData))]
        public void SyntaxFact_GetText_RoundTrips(SyntaxType type)
        {
            string text = SyntaxFacts.GetText(type);
            if (text == null)
                return;

            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);

            Assert.Equal(type, token.Type);
            Assert.Equal(text, token.Text);
        }

        public static IEnumerable<object[]> GetSyntaxTypeData()
        {
            var types = (SyntaxType[])Enum.GetValues(typeof(SyntaxType));
            foreach (var type in types)
                yield return new object[] { type };
        }
    }
}