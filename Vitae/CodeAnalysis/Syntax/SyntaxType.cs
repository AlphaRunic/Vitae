namespace Vitae.CodeAnalysis.Syntax
{
    public enum SyntaxType
    {
        // Special Tokens
        InvalidToken,
        EOFToken,

        // Tokens
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        PercentToken,
        CaratToken,
        OpenParenToken,
        ClosedParenToken,
        BangToken,
        AmpersandToken,
        PipeToken,
        EqualToToken,
        NotEqualToToken,
        IdentifierToken,
        EqualsToken,
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,
        NameExpression,
        AssignmentExpression,

        // Nodes

        CompilationUnit,

        // Keywords
        TrueKeyword,
        FalseKeyword
    }
}
