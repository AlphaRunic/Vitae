using System;
using System.Collections.Generic;
using Vitae.CodeAnalysis;
using Vitae.CodeAnalysis.Syntax;
using Xunit;

namespace Vitae.Tests.CodeAnalysis
{
    public class EvaluationTest
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("+1", 1)]
        [InlineData("-1", -1)]
        [InlineData("14 + 12", 26)]
        [InlineData("12 - 3", 9)]
        [InlineData("4 * 2", 8)]
        [InlineData("9 / 3", 3)]
        [InlineData("(10)", 10)]
        [InlineData("12 == 3", false)]
        [InlineData("5 == 5", true)]
        [InlineData("12 != 3", true)]
        [InlineData("5 != 5", false)]
        [InlineData("true == false", false)]
        [InlineData("false == false", true)]
        [InlineData("false != false", false)]
        [InlineData("true != false", true)]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("!true", false)]
        [InlineData("!false", true)]
        [InlineData("false & true", false)]
        [InlineData("false | true", true)]
        [InlineData("(a = 10) * a", 100)]
        public void Evaluator(string text, object expectedValue)
        {
            SyntaxTree syntaxTree = SyntaxTree.Parse(text);
            Compilation compilation = new Compilation(syntaxTree);
            Dictionary<VariableSymbol, object> variables = new Dictionary<VariableSymbol, object>();
            EvaluationResult result = compilation.Evaluate(variables);

            Assert.Empty(result.Diagnostics);
            Assert.Equal(expectedValue, result.Value);
        }
    }
}