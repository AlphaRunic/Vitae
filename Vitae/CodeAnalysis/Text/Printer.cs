using System;
using System.IO;

public class Printer
{
    public static void PrintInColor(TextWriter writer, ConsoleColor color, object value, bool line = false)
    {
        Console.ForegroundColor = color;

        if (!line)
            writer.Write(value);        
        else
            writer.WriteLine(value);

        Console.ResetColor();
    }
}