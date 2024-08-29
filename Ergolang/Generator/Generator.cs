using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Generator
{
    [Generator]
    public class Generator : IIncrementalGenerator
    {

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var pipeline = context.AdditionalTextsProvider
                .Where(static text => true)
                .Select(static (text, token) => (fileName: Path.GetFileNameWithoutExtension(text.Path), content: text.GetText(token)?.ToString()));


            context.RegisterSourceOutput(pipeline, (ctx, filenameAndContent) =>
            {
                ctx.AddSource($"{filenameAndContent.fileName}.g.cs", SourceText.From(GenerateExpr(filenameAndContent), Encoding.UTF8));
                
            });
        }

        private string GenerateExpr((string fileName, string content) tpl)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace Ergolang;");
            sb.AppendLine($"public abstract record {tpl.fileName} {{");
            var lines = tpl.content.Split('\n');

            sb.AppendLine("public interface IVisitor<T> {");

            foreach (var line in lines)
            {
                var (name, param) = (line.Split(':')[0].Trim(), line.Split(':')[1].Trim());

                sb.AppendLine($$"""
                                T Visit({{tpl.fileName}}.{{name}} {{tpl.fileName.ToLower()}});
                                """);
            }

            sb.AppendLine("}");

            sb.AppendLine($"public abstract T Accept<T>(IVisitor<T> visitor);");

            foreach (var line in lines)
            {
                var (name, param) = (line.Split(':')[0].Trim(), line.Split(':')[1].Trim());

                sb.AppendLine($$"""
                                public record {{name}}({{param}}) : {{tpl.fileName}}() {
                                     public override T Accept<T>(IVisitor<T> visitor){
                                         return visitor.Visit(this);
                                     }
                                }
                                """);
            }


            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}