namespace Ergolang;
public abstract partial record Stmt {
public record Expression(Expr expression) : Stmt() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
public record Print(Expr expression) : Stmt() {
     public override T Accept<T>(IVisitor<T> visitor){
         return visitor.Visit(this);
     }
}
}
