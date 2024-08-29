namespace Ergolang;

public abstract partial record Expr
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}