using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp;

namespace Pluginerr
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var syntaxTree = CSharpSyntaxTree.ParseText(@"using System; using System.IO; class Desu{void main(){Console.WriteLine(""hello"");}}");
            List<MetadataReference> references = new List<MetadataReference>();
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            //var assemblies = System.Reflection.Assembly.GetExecutingAssembly().GetReferencedAssemblies()[0].;

            references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Private.CoreLib.dll")));
            references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")));
            references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Console.dll")));
            references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")));

            CSharpCompilation compilation = CSharpCompilation.Create(
                "assemblyName",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var dllStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var emitResult = compilation.Emit("test.dll");
                //var emitResult = compilation.Emit(dllStream, pdbStream);
                if (!emitResult.Success)
                {
                    Console.WriteLine(emitResult.Diagnostics.ToString());
                    // emitResult.Diagnostics
                    Console.WriteLine("hello");
                }
            }
        }
    }
}
