using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SokairykFramework.CodeGeneration
{
    public class CSharpLanguage : ILanguageService
    {
        public Compilation CreateLibraryCompilation(string code, string assemblyName, IEnumerable<string> assemblyPaths = null, bool enableOptimisations = false)
        {
            var compilationOptionOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: enableOptimisations ? OptimizationLevel.Release : OptimizationLevel.Debug,
                allowUnsafe: true);

            var portableExecutableReferences = assemblyPaths != null ?
                assemblyPaths.Select(a => MetadataReference.CreateFromFile(a))
                : new PortableExecutableReference[] { };
            var tyet = MetadataReference.CreateFromFile(assemblyPaths.First());

            return CSharpCompilation.Create(assemblyName, options: compilationOptionOptions)
                                    .AddReferences(portableExecutableReferences)
                                    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(languageVersion: LanguageVersion.Latest, kind: SourceCodeKind.Regular)));
        }

        public Stream GetStreamOfCompilation(Compilation compilation, out Diagnostic[] diagnostics)
        {
            var stream = new MemoryStream();

            var result = compilation.Emit(stream);

            diagnostics = result.Diagnostics.ToArray();

            if (result.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
                
            return Stream.Null;
        }
    }
}
