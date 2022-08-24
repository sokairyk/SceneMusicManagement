using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;

namespace Sokairyk.DynamicCode.CSharp
{
    public class CSharpCompilationManager : CompilationManager
    {
        private readonly ILogger _logger;

        public CSharpCompilationManager(ILogger<CSharpCompilationManager> logger) : base(logger)
        {
            _logger = logger;
        }

        public override Compilation CreateAssemblyCompilation(string code, string assemblyName = null, IEnumerable<string> assemblyPaths = null, bool enableOptimisations = false)
        {
            var compilationOptionOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: enableOptimisations ? OptimizationLevel.Release : OptimizationLevel.Debug,
                allowUnsafe: true);

            var portableExecutableReferences = assemblyPaths != null ?
                assemblyPaths.Select(a => MetadataReference.CreateFromFile(a))
                : new PortableExecutableReference[] { };

            return CSharpCompilation.Create(assemblyName ?? $"Random_{Guid.NewGuid().ToString().Replace("-", "")}", options: compilationOptionOptions)
                                    .AddReferences(portableExecutableReferences)
                                    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(languageVersion: LanguageVersion.Latest, kind: SourceCodeKind.Regular)));
        }
    }
}
