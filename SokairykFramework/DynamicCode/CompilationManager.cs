using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SokairykFramework.DynamicCode
{
    public abstract class CompilationManager : ILanguageCompilation
    {
        public abstract Compilation CreateAssemblyCompilation(string code, string assemblyName = null, IEnumerable<string> assemblyReferences = null, bool enableOptimisations = false);

        public Stream GetAssemblyStream(Compilation compilation, out Diagnostic[] diagnostics)
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

        public bool CompileCodeAndLoadAssemblyIntoContext(string code, IEnumerable<string> assemblyReferences)
        {
            var stream = GetAssemblyStream(CreateAssemblyCompilation(code, assemblyReferences: assemblyReferences), out var diagnostics);

            if (stream == Stream.Null) return false;

            try
            {
                System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(stream);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
