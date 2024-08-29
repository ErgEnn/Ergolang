namespace Ergolang
{
    internal class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        public void Interpret(IList<Stmt> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError e)
            {
                Lang.RuntimeError(e);
            }
        }

        private object? Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public object Visit(Expr.Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double) left > (double) right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double) left >= (double) right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double) left < (double) right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double) left <= (double) right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.PLUS:
                    if (left is double ld && right is double rd)
                        return ld + rd;
                    if (left is string ls && right is string rs)
                        return ls + rs;
                    throw new RuntimeError(expr.Operator, "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double) left / (double) right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double) left * (double) right;
            }

            throw new InvalidOperationException();
        }

        public object? Visit(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }
        
        public object Visit(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object Visit(Expr.Unary expr)
        {
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double) right;
            }

            throw new InvalidOperationException();
        }

        public object Visit(Stmt.ExpressionStm stmt)
        {
            Evaluate(stmt.Expression);
            return typeof(void);
        }

        public object Visit(Stmt.Print stmt)
        {
            var value = Evaluate(stmt.Expression);
            Console.WriteLine(Stringify(value));
            return typeof(void);
        }

        private void CheckNumberOperand(Token @operator, object? operand)
        {
            if (operand is double) return;
            throw new RuntimeError(@operator, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token @operator, object? left, object? right)
        {
            if (left is double && right is double) return;
            throw new RuntimeError(@operator, "Operand must be a number.");
        }

        private bool IsTruthy(object? obj)
        {
            if (obj == null) return false;
            if (obj is bool b) return b;
            return true;
        }

        private bool IsEqual(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is double)
            {
                var txt = obj.ToString();
                if (txt.EndsWith(".0")) txt = txt.Replace(".0", "");
                return txt;
            }

            return obj!.ToString();
        }
    }
}
