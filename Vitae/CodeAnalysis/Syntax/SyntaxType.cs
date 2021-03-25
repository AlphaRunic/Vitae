namespace Vitae.CodeAnalysis.Syntax
{
    public enum SyntaxType
    {
        // Special Tokens
        Invalid,
        EOF,

        // Tokens
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
        Bang,
        Ampersand,
        Pipe,
        EqualTo,
        NotEqualTo,
        Identifier,
        Assignment,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,
        NameExpression,
        AssignmentExpression,

        // Keywords
        TrueKeyword,
        FalseKeyword
    }
}
