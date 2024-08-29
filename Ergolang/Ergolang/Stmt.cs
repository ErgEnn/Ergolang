namespace Ergolang;

public abstract partial record Stmt
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}