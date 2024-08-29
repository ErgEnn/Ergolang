namespace Ergolang;
public abstract partial record Expr {
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
