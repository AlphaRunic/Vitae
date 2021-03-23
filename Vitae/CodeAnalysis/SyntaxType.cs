namespace Vitae.CodeAnalysis {
    public enum SyntaxType {
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
        
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}
