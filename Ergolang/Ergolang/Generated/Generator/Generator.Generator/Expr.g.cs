namespace Ergolang;
public abstract record Expr {
public interface IVisitor<T> {
T Visit(Expr.Assign expr);
T Visit(Expr.Binary expr);
T Visit(Expr.Grouping expr);
T Visit(Expr.Literal expr);
T Visit(Expr.Unary expr);
T Visit(Expr.Variable expr);
}
public abstract T Accept<T>(IVisitor<T> visitor);
public record Assign(Token Name, Expr Value) : Expr() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
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
public record Variable(Token Name) : Expr() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
}
