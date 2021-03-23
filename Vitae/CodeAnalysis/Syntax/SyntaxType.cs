namespace Vitae.CodeAnalysis.Syntax
{
    public enum SyntaxType
    {
        Invalid,
        EOF,

        Number,
        Whitespace,
        Plus,
        Minus,
        Multiply,
        Divide,
        Modulo,
        Power,
        OpenParen,
        ClosedParen,
        Identifier,
        
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,

        True,
        False
    }
}
