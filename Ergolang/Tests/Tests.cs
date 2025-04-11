using Ergolang;
using FluentAssertions;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Lang.Run("""
                     var a = 1;
                     var b = 2;
                     a = 3;
                     print a + b;
                     """);
        }
        
        [Test]
        public void Test11()
        {
            Lang.Run("""
                     var a = 5;
                     {
                       var a = a + 2;
                       print a;
                     }
                     """);
        }

        [Test]
        public void Test2()
        {
            Lang.Run("""
                     print 2+1;
                     print "one";
                     print true;
                     """);
        }

        [Test]
        public void Test3()
        {
            var expr = new Expr.Binary(
                new Expr.Binary(
                    new Expr.Literal(1),
                    new Token(TokenType.PLUS, "+".AsMemory(), null, 1),
                    new Expr.Literal(2)
                ),
                new Token(TokenType.STAR, "*".AsMemory(), null, 1),
                new Expr.Binary(
                    new Expr.Literal(4),
                    new Token(TokenType.MINUS, "-".AsMemory(), null, 1),
                    new Expr.Literal(3)
                    )
            );

            (new RpnPrinter().Print(expr)).Should().Be("1 2 + 4 3 - *");
        }
    }
}