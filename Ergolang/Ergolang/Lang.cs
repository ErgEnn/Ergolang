namespace Ergolang
{
    public class Lang
    {
        private static readonly Interpreter interpreter = new ();
        private static bool hadError = false;
        private static bool hadRuntimeError = false;

        static void Main(string[] args)
        {
            Run(args[0]);
        }

        public static void Run(string source)
        {
            
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();

            var parser = new Parser(tokens);
            var expression = parser.Parse();

            if(hadError) return;

            interpreter.Interpret(expression);
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            hadError = true;
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, "at end", message);
            }
            else
            {
                Report(token.Line, $"at '{token.Lexeme}'", message);
            }
        }

        public static void RuntimeError(RuntimeError error)
        {
            Console.Error.WriteLine($"{error.Message}\n[line {error.Token.Line}]");
            hadRuntimeError = true;
        }
    }
}
