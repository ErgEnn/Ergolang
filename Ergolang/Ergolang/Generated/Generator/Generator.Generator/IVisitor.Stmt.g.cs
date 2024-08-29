namespace Ergolang;
public partial interface IVisitor<T> {
T Visit(Stmt.Expression expr);
T Visit(Stmt.Print expr);
}
