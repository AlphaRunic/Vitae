using System;
using System.Collections;
using System.Collections.Generic;
using Vitae.CodeAnalysis.Syntax;
using Vitae.CodeAnalysis.Text;

namespace Vitae.CodeAnalysis
{
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() =>_diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }

        private void Report(TextSpan span, string msg)
        {
            var diagnostic = new Diagnostic(span, "Error: " + msg);
            _diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextSpan span, string text, Type type)
        {
            string msg = $"the number {text} isn't a valid {type}.";
            Report(span, msg);
        }

        public void ReportInvalidCharacter(int pos, char character)
        {
            TextSpan span = new TextSpan(pos, 1);
            string msg = $"invalid character input: '{character}'.";
            Report(span, msg);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxType actualType, SyntaxType expectedType)
        {
            string msg = $"unexpected token <{actualType}>, expected <{expectedType}>.";
            Report(span, msg);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operandType)
        {
            string msg = $"unary operator '{operatorText}' is not defined for type {operandType}.";
            Report(span, msg);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
        {
            string msg = $"binary operator '{operatorText}' is not defined for types {leftType} and {rightType}.";
            Report(span, msg);
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            string msg = $"variable '{name}' doesn't exist.";
            Report(span, msg);
        }
    }
}