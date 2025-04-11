namespace Ergolang;

internal class Environment
{
    private readonly Environment? _enclosing;
    private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

    public Environment()
    {
        _enclosing = null;
    }

    public Environment(Environment enclosing)
    {
        _enclosing = enclosing;
    }

    public void Define(string name, object value)
    {
        _values.Add(name, value);
    }

    public object Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme.ToString(), out var value))
        {
            return value;
        }

        if (_enclosing is not null)
        {
            return _enclosing.Get(name);
        }

        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public void Assign(Token name, object? value)
    {
        var nameStr = name.Lexeme.ToString();
        if (_values.ContainsKey(nameStr))
        {
            _values[nameStr] = value;
            return;
        }

        if (_enclosing is not null)
        {
            _enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeError(name, $"Undefined variable '{nameStr}'.");
    }
}