using System;
using System.Collections.Immutable;

namespace Vitae.CodeAnalysis.Text
{
    public sealed class SourceText
    {
        private readonly string _text;

        public ImmutableArray<TextLine> Lines { get; }

        private SourceText(string text)
        {
            Lines = ParseLines(this, text);
            _text = text;
        }

        public char this[int index] => _text[index];
        public int Length => _text.Length;

        public int GetLineIndex(int pos)
        {
            var lower = 0;
            var upper = Lines.Length - 1;

            while (lower <= upper)
            {
                var index = lower + (upper - lower) / 2;
                var start = Lines[index].Start;

                if (pos == start)
                    return index;
                
                if (start > pos)
                {
                    upper = index - 1;
                }
                else
                {
                    lower = index + 1;
                }
            }

            return lower - 1;
        }

        private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
        {
            var result = ImmutableArray.CreateBuilder<TextLine>();

            var pos = 0;
            var lineStart = 0;

            while(pos < text.Length)
            {
                var lineBreakWidth = GetLineBreakWidth(text, pos);
                
                if (lineBreakWidth == 0)
                {
                    pos++;
                }
                else
                {
                    AddLine(result, sourceText, pos, lineStart, lineBreakWidth);
                    pos += lineBreakWidth;
                    lineStart = pos;
                }
            }

            if (pos >= lineStart)
                AddLine(result, sourceText, pos, lineStart, 0);

            return result.ToImmutable();
        }

        private static void AddLine(ImmutableArray<TextLine>.Builder result, SourceText sourceText, int pos, int lineStart, int lineBreakWidth)
        {
            var lineLength = pos - lineStart;
            var lengthIncludingBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lengthIncludingBreak);
            result.Add(line);
        }

        private static int GetLineBreakWidth(string text, int pos)
        {
            var c = text[pos];
            var l = pos + 1 >= text.Length ? '\0' : text[pos + 1];

            if (c == '\r' && l == '\n')
                return 2;
            else if (c == '\r' || c == '\n')
                return 1;
            else
                return 0;
        }

        public static SourceText From(string text)
        {
            return new SourceText(text);
        }

        public override string ToString() => _text;
        public string ToString(int start, int length) => _text.Substring(start, length);
        public string ToString(TextSpan span) => _text.Substring(span.Start, span.Length);
    }
}