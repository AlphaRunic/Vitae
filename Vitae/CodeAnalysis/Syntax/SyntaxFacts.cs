namespace Vitae.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.Plus:
                case SyntaxType.Minus:
                    return 4;
                
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.Power:
                    return 3;

                case SyntaxType.Modulo:
                case SyntaxType.Multiply:
                case SyntaxType.Divide:
                    return 2;

                case SyntaxType.Plus:
                case SyntaxType.Minus:
                    return 1;
                
                default:
                    return 0;
            }
        }
    }
}