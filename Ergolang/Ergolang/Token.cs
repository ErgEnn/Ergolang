namespace Ergolang;

public class Token
{
    public TokenType Type { get; init; }
    public ReadOnlyMemory<char> Lexeme { get; init; }
    public object Literal { get; init; }
    public int Line { get; init; }

    public Token(TokenType type, ReadOnlyMemory<char> lexeme, object literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal}";
    }
}