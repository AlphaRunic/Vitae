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
            var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            var op1Text = SyntaxFacts.GetText(op1);
            var op2Text = SyntaxFacts.GetText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var expr = SyntaxTree.Parse(text).Root;

            if (op1Precedence >= op2Precedence)
            {
                using (var e = new AssertingEnumerator(expr))
                {
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "a");
                    e.AssertToken(op1, op1Text);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "b");
                    e.AssertToken(op2, op2Text);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "c");
                }
            }
            else
            {
                using (var e = new AssertingEnumerator(expr))
                {
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "a");
                    e.AssertToken(op1, op1Text);

                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "b");
                    e.AssertToken(op2, op2Text);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "c");
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        public void Parser_UnaryExpression_HonorsPrecedences(SyntaxType unaryType, SyntaxType binaryType)
        {
            var unaryPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(unaryType);
            var binaryPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(binaryType);
            var unaryText = SyntaxFacts.GetText(unaryType);
            var binaryText = SyntaxFacts.GetText(binaryType);
            var text = $"{unaryText} a {binaryText} b";
            var expr = SyntaxTree.Parse(text).Root;

            if (!(unaryPrecedence >= binaryPrecedence))
            {
                using (var e = new AssertingEnumerator(expr))
                {
                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.UnaryExpression);
                    e.AssertToken(unaryType, unaryText);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "a");
                    e.AssertToken(binaryType, binaryText);

                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "b");
                }
            }
            else
            {
                using (var e = new AssertingEnumerator(expr))
                {
                    e.AssertNode(SyntaxType.UnaryExpression);
                    e.AssertToken(unaryType, unaryText);

                    e.AssertNode(SyntaxType.BinaryExpression);
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "a");
                    e.AssertToken(binaryType, binaryText);
                    
                    e.AssertNode(SyntaxType.NameExpression);
                    e.AssertToken(SyntaxType.IdentifierToken, "b");
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
        public static IEnumerable<object[]> GetUnaryOperatorPairsData()
        {
            foreach (var unary in SyntaxFacts.GetUnaryOperatorTypes())
            {
                foreach (var binary in SyntaxFacts.GetBinaryOperatorTypes())
                {
                    yield return new object[] { unary, binary };
                    yield break;
                }
            }
        }
    }
}