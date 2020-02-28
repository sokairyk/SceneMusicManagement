using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SokairykFramework.DynamicCode
{
    public class CSharpCompilationManager : CompilationManager
    {
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
