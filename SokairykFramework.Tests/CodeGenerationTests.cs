using NUnit.Framework;
using SokairykFramework.CodeGeneration;
using SokairykFramework.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SokairykFramework.Tests
{
    public class CodeGenerationTests
    {
        [Test]

        public void CompilationTest()
        {
            var simpleClass = @"
        using System;
        namespace ExampleNS {

        public class ExampleClass {
            
            private readonly string _message;

            public ExampleClass()
            {
                _message = ""Hello World"";
            }

            public string getMessage()
            {
                return _message;
            }
          }
        }";
           
            var langService = new CSharpLanguage();
            var assmblyLoc = ReflectionExtensions.FindAssembliesFromTypes(new[] { typeof(string) });
            var cv = @" C:\Program Files\dotnet\sdk\NuGetFallbackFolder\microsoft.netcore.app\2.1.0\ref\netcoreapp2.1\System.Runtime.dll";
            var comp =  langService.CreateLibraryCompilation(simpleClass, "InMemoryExample", assmblyLoc);
            var stream = langService.GetStreamOfCompilation(comp, out var diagnostics);

            using (var file = new FileStream(Path.Combine(AppContext.BaseDirectory, "test.dll"), FileMode.CreateNew))
            {
                stream.CopyTo(file);
            }

            //var b = new ExampleNS.ExampleClass();

            //Assert.IsNotNull(stream);

        }

    }
}
