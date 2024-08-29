using System.Text;

namespace Ergolang;

public class AstPrinter : IVisitor<string>
{
    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }

    public string Visit(Expr.Binary expr)
    {
        return Parenthesize(expr.Operator.Lexeme.ToString(), expr.Left, expr.Right);
    }

    public string Visit(Expr.Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public string Visit(Expr.Literal expr)
    {
        if (expr.Value == null) return "nil";
        return expr.Value.ToString();
    }

    public string Visit(Expr.Unary expr)
    {
        return Parenthesize(expr.Operator.Lexeme.ToString(), expr.Right);
    }

    private string Parenthesize(string name, params Expr[] exprs)
    {
        var sb = new StringBuilder();

        sb.Append($"({name}");
        foreach (var expr in exprs)
        {
            sb.Append($" {expr.Accept(this)}");
        }

        sb.Append(")");

        return sb.ToString();
    }
}