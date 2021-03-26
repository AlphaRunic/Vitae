using System.Collections.Generic;

namespace Vitae.CodeAnalysis.Syntax
{
    public sealed class NameExpression : ExpressionSyntax
    {
        public NameExpression(Token identifier)
        {
            Identifier = identifier;
        }

        public override SyntaxType Type => SyntaxType.NameExpression;
        public Token Identifier { get; }
    }
}