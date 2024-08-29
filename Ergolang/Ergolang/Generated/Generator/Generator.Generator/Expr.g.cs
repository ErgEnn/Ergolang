namespace Ergolang;
public abstract record Expr {
public interface IVisitor<T> {
T Visit(Expr.Binary expr);
T Visit(Expr.Grouping expr);
T Visit(Expr.Literal expr);
T Visit(Expr.Unary expr);
}
public abstract T Accept<T>(IVisitor<T> visitor);
public record Binary(Expr Left, Token Operator, Expr Right) : Expr() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record Grouping(Expr Expression) : Expr() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record Literal(object Value) : Expr() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record Unary(Token Operator, Expr Right) : Expr() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
}
