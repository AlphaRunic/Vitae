namespace vc {
    public class Token {
        public SyntaxType Type { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public Token(SyntaxType type, int pos, string text, object value) {
            Type = type;
            Position = pos;
            Text = text;
            Value = value;
        }
    }
}
