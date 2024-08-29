namespace Ergolang;
public interface IVisitor<T> {
T Visit(Expr.Binary expr);
T Visit(Expr.Grouping expr);
T Visit(Expr.Literal expr);
T Visit(Expr.Unary expr);
}
