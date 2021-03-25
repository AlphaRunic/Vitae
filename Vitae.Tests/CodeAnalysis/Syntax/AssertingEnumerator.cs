using System;
using System.Collections.Generic;
using System.Linq;
using Vitae.CodeAnalysis.Syntax;
using Xunit;

namespace Vitae.Tests.CodeAnalysis.Syntax
{
    internal sealed class AssertingEnumerator : IDisposable
    {
        private IEnumerator<SyntaxNode> _enumerator;
        private bool _hasErrors;

        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
        }

        private bool MarkFailed()
        {
            _hasErrors = true;
            return false;
        }

        public void Dispose()
        {
            if (!_hasErrors)
                Assert.False(_enumerator.MoveNext());

            _enumerator.Dispose();
        }

        private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                    stack.Push(child);
            }
        }

        public void AssertNode(SyntaxType type)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                Assert.Equal(type, _enumerator.Current.Type);
                Assert.IsNotType<Token>(_enumerator.Current);
            }
            catch when (MarkFailed())
            {
                _hasErrors = true;
                throw;
            }
        }

        public void AssertToken(SyntaxType type, string text)
        {
            try
            {
                Assert.True(_enumerator.MoveNext());
                var token = Assert.IsType<Token>(_enumerator.Current);

                Assert.Equal(type, token.Type);
                Assert.Equal(text, token.Text);
            }
            catch when (MarkFailed())
            {
                _hasErrors = true;
                throw;
            }
        }
    }
}