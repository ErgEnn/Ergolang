namespace Ergolang.Stmt;
public partial interface IVisitor<T> {
T Visit(Stmt.ExpressionStm stmt);
T Visit(Stmt.Print stmt);
}
