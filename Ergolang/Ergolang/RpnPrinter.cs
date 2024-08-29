namespace Ergolang;

// Reverse polish notation
public class RpnPrinter : IVisitor<string>
{
    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }

    public string Visit(Expr.Binary expr)
    {
        return $"{expr.Left.Accept(this)} {expr.Right.Accept(this)} {expr.Operator.Lexeme}";
    }

    public string Visit(Expr.Grouping expr)
    {
        throw new NotImplementedException();
    }

    public string Visit(Expr.Literal expr)
    {
        return expr.Value.ToString();
    }

    public string Visit(Expr.Unary expr)
    {
        throw new NotImplementedException();
    }
}