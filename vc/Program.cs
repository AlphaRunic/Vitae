using System;

namespace vc {
    class Program {
        static void Main(string[] args) {
            while(true) {
                Console.Write("vitae >> ");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return;

                // if (line == "1 + 2 * 3")
                //     Console.WriteLine("7");
                // else
                //     Console.WriteLine("Error: Invalid expression.");
            }
        }
    }

    class Token {

        public string _type;
        public id

        public Token(string type, id value) {
            _type = type;
            _value = value;
        }
    }

    class Lexer {

        private readonly string _text;
        private int _pos;

        public Lexer(string text) {
            _text = text;
        }

        public Token NextToken() {

        }
    }
}
