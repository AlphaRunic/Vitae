using System.Collections.Generic;
using Vitae.CodeAnalysis.Syntax;
using Xunit;

namespace Vitae.Tests.CodeAnalysis.Syntax
{
    public class ParserTest
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void Parser_BinaryExpression_HonorsPrecedences(SyntaxType op1, SyntaxType op2)
        {
            int op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            int op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            string op1Text = SyntaxFacts.GetText(op1);
            string op2Text = SyntaxFacts.GetText(op2);
            string text = $"a {op1Text} b {op2Text} c";
            ExpressionSyntax expr = SyntaxTree.Parse(text).Root;

            if (op1Precedence >= op2Precedence)
            {
                using (var e = new AssertingEnumerator(expr))
                {
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.Identifier, "a");
                    e.AssertToken(op1, op1Text);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.Identifier, "b");
                    e.AssertToken(op2, op2Text);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.Identifier, "c");
                }
            }
            else
            {
                using (var e = new AssertingEnumerator(expr))
                {
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.Identifier, "a");
                    e.AssertToken(op1, op1Text);

                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.Identifier, "b");
                    e.AssertToken(op2, op2Text);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.Identifier, "c");
                }
            }
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (var op1 in SyntaxFacts.GetBinaryOperatorTypes())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorTypes())
                {
                    yield return new object[] { op1, op2 };
                    yield break;
                }
            }
        }
    }
}