namespace Ergolang;
public abstract record Stmt {
public interface IVisitor<T> {
T Visit(Stmt.Block stmt);
T Visit(Stmt.ExpressionStm stmt);
T Visit(Stmt.Print stmt);
T Visit(Stmt.Var stmt);
}
public abstract T Accept<T>(IVisitor<T> visitor);
public record Block(IList<Stmt> Statements) : Stmt() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record ExpressionStm(Expr Expression) : Stmt() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record Print(Expr Expression) : Stmt() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record Var(Token Name, Expr? Initializer) : Stmt() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
}
