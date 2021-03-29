using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vitae.CodeAnalysis;
using Vitae.CodeAnalysis.Binding;
using Vitae.CodeAnalysis.Syntax;
using Vitae.CodeAnalysis.Text;

namespace Vitae
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var showTree = false;
            var variables = new Dictionary<VariableSymbol, object>();
            var textBuilder = new StringBuilder();

            while (true)
            {
                if (textBuilder.Length == 0)
                    Console.Write("Vitae >> ");
                else
                    Console.Write("| ");

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrEmpty(input);

                if (textBuilder.Length == 0)
                {
                    if (isBlank)
                        break;
                    else if (input == "#showTrees")
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
                        continue;
                    }
                    else if (input == "#cls")
                    {
                        Console.Clear();
                        continue;
                    }
                }
                
                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();
                var syntaxTree = SyntaxTree.Parse(text);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                if (!isBlank && syntaxTree.Diagnostics.Any())
                    continue;

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    syntaxTree.Root.WriteTo(Console.Out);
                    Console.ResetColor();
                }

                if (!result.Diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        var lineNumber = lineIndex + 1;
                        var line = syntaxTree.Text.Lines[lineIndex];
                        var character = diagnostic.Span.Start - line.Start + 1;
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineNumber}:{character}) ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        TextSpan prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        TextSpan suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

                        string prefix = syntaxTree.Text.ToString(prefixSpan);
                        string error = syntaxTree.Text.ToString(diagnostic.Span);
                        string suffix = syntaxTree.Text.ToString(suffixSpan);

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }

                textBuilder.Clear();
            }
        }
    }
}