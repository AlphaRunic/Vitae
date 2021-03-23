using System;
using System.Collections.Generic;
using System.Linq;

using Vitae.CodeAnalysis;
using Vitae.CodeAnalysis.Binding;
using Vitae.CodeAnalysis.Syntax;

namespace Vitae
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            bool showTree = false;

            while (true)
            {
                Console.Write("Vitae >> ");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
                    continue;
                }
                else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                SyntaxTree syntaxTree = SyntaxTree.Parse(line);
                Binder binder= new Binder();
                BoundExpression boundExpression = binder.BindExpression(syntaxTree.Root);
                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
                
                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    
                    foreach (var diagnostic in diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ResetColor();
                }
                else
                {
                    Evaluator eval = new Evaluator(boundExpression);
                    var res = eval.Evaluate();
                    Console.WriteLine(res);
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "")
        {
            Console.Write(indent);
            Console.Write(node.Type);

            if (node is Token t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += "   ";

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent);
        }
    }
}