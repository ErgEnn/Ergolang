using static Ergolang.TokenType;

namespace Ergolang;

internal class Scanner
{
    private readonly string _source;
    private readonly List<Token> _tokens = new();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Scanner(string source)
    {
        _source = source;
    }

    public IEnumerable<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(EOF, ReadOnlyMemory<char>.Empty, null, _line));
        return _tokens;
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(LEFT_PAREN); break;
            case ')': AddToken(RIGHT_PAREN); break;
            case '{': AddToken(LEFT_BRACE); break;
            case '}': AddToken(RIGHT_BRACE); break;
            case ',': AddToken(COMMA); break;
            case '.': AddToken(DOT); break;
            case '-': AddToken(MINUS); break;
            case '+': AddToken(PLUS); break;
            case ';': AddToken(SEMICOLON); break;
            case '*': AddToken(STAR); break;

            case '!':
                AddToken(Match('=') ? BANG_EQUAL : BANG);
                break;
            case '=':
                AddToken(Match('=') ? EQUAL_EQUAL : EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? LESS_EQUAL : LESS);
                break;
            case '>':
                AddToken(Match('=') ? GREATER_EQUAL : GREATER);
                break;

            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                }
                else
                {
                    AddToken(SLASH);
                }

                break;

            case ' ':
            case '\r':
            case '\t':
                //Ignore whitespace
                break;

            case '\n':
                _line++;
                break;

            case '"': String(); break;

            default:
                if (char.IsAsciiDigit(c)) // Just to avoid typing cases for all numbers
                {
                    Number();
                }else if (char.IsAsciiLetter(c)) // All regular letters (note: we don't allow underscore)
                {
                    Identifier();
                }
                else
                {
                    Lang.Error(_line, $"Unexpected character: '{c}'");
                }
                break;
        }
    }

    private static readonly IDictionary<string, TokenType> ReservedKeywords = new Dictionary<string, TokenType>()
    {
        {"and", AND},
        {"class", CLASS},
        {"else", ELSE},
        {"false", FALSE},
        {"for", FOR},
        {"fun", FUN},
        {"if", IF},
        {"nil", NIL},
        {"or", OR},
        {"print", PRINT},
        {"return", RETURN},
        {"super", SUPER},
        {"this", THIS},
        {"true", TRUE},
        {"var", VAR},
        {"while", WHILE}
    };

    private void Identifier()
    {
        while (char.IsAsciiLetterOrDigit(Peek())) Advance();

        var text = _source[_start.._current];
        if (!ReservedKeywords.TryGetValue(text, out var type)) type = IDENTIFIER;
        AddToken(type);
    }

    private void Number()
    {
        // Consume all digits
        while (char.IsAsciiDigit(Peek())) Advance();

        // If dot is detected, check if followed by digits, then consume them too
        if (Peek() == '.' && char.IsAsciiDigit(PeekNext()))
        {
            Advance(); // Consume the .

            while (char.IsAsciiDigit(Peek())) Advance();
        }

        AddToken(NUMBER, Double.Parse(_source[_start.._current]));
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Lang.Error(_line, "Unterminated string");
            return;
        }

        Advance(); // Consume closing "

        var value = _source[(_start + 1)..(_current - 1)]; //+1 and -1 to strip surrounding quotes
        AddToken(STRING, value);
    }

    private char PeekNext()
    {
        if (_current + 1 >= _source.Length) return '\0';
        return _source[_current + 1];
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _source[_current];
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;

        _current++;
        return true;
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object literal)
    {
        var text = _source.AsMemory(_start.._current);
        _tokens.Add(new Token(type, text, literal, _line));
    }
}