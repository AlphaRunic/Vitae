using System;

namespace vc {
    public class Program {
        static void Main(string[] args) {
            while(true) {
                Console.Write("vitae >> ");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return;
                
                Lexer lexer = new Lexer(line);
                while (true) {
                    Token token = lexer.NextToken();
                    if (token.Type == SyntaxType.EOF)
                        break;
                    
                    Console.Write($"{token.Type}: '{token.Text}' | Value: ");
                    if (token.Value != null)
                        Console.Write($" {token.Value}");
                    
                    Console.WriteLine();
                }
            }
        }
    }
}