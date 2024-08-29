using System.IO;
using System.Linq;
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
                //ctx.AddSource($"{filenameAndContent.fileName}.g.cs", SourceText.From(GenerateExpr(filenameAndContent), Encoding.UTF8));
                ctx.AddSource($"IVisitor.{filenameAndContent.fileName}.g.cs", SourceText.From(GenerateIVisitor(filenameAndContent), Encoding.UTF8));
            });
        }

        private string GenerateIVisitor((string fileName, string content) tpl)
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace Ergolang;");
            sb.AppendLine("public partial interface IVisitor<T> {");

            var lines = tpl.content.Split('\n').Where(s => s.Length > 5);
            foreach (var line in lines)
            {
                var (name, param) = (line.Split(':')[0].Trim(), line.Split(':')[1].Trim());

                sb.AppendLine($$"""
                                T Visit({{tpl.fileName}}.{{name}} expr);
                                """);
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private string GenerateExpr((string fileName, string content) tpl)
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace Ergolang;");
            sb.AppendLine($"public abstract partial record {tpl.fileName} {{");

            var lines = tpl.content.Split('\n').Where(s => s.Length > 5);
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