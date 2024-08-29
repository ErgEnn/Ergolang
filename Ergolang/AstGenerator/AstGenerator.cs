using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AstGenerator
{
    [Generator]
    public class AstGenerator : IIncrementalGenerator
    {
        private string test =
            """
            namespace Ergolang
            {
                abstract partial record Expr {
                    public abstract record Grouping(Expr Expression) : Expr();
                }
            }
            """;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var pipeline = context.AdditionalTextsProvider
                .Where(static text => true)
                .Select(static (text, token) =>
                {
                    return (name: Path.GetFileNameWithoutExtension(text.Path), text: text.GetText(token));
                });

            context.RegisterSourceOutput(pipeline, (ctx, text) =>
            {
                ctx.AddSource($"{text.name}.g.cs", SourceText.From(test));
            });
        }
    }
}
