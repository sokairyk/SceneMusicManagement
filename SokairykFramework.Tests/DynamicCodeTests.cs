using Microsoft.CodeAnalysis;
using NUnit.Framework;
using SokairykFramework.DynamicCode;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SokairykFramework.Tests
{
    public class DynamicCodeTests
    {
        private static readonly string _simpleCode = @"
using System;
namespace ExampleNS {
    public class ExampleClass {
            
        private readonly string _message;

        public ExampleClass()
        {
            _message = ""Hello World"";
        }

        public ExampleClass(string message)
        {
            _message = message;
        }

        public string GetMessage()
        {
            return _message;
        }
    }
}";
        private static string _clonedCode => _simpleCode.Replace("ExampleClass", "ExampleClassClone");
        private static string _clownedCode => _simpleCode.Replace("ExampleClass", "ExampleClassClown");
        private static string _invalidCode => _simpleCode.Replace("_message = \"Hello World\";", "_message = Hello World;");
        private static readonly string[] _referencedAssemblyPaths = new[] { typeof(string).GetTypeInfo().Assembly.Location };

        [Test]
        public void InvalidCodeCompilationTest()
        {
            var cSharpLang = new CSharpCompilationManager();
            var compilation = cSharpLang.CreateAssemblyCompilation(_invalidCode);

            var assemblyStream = cSharpLang.GetAssemblyStream(compilation, out var diagnostics);

            Assert.AreEqual(assemblyStream, Stream.Null);
            Assert.IsNotNull(diagnostics.FirstOrDefault(d => d.Severity == DiagnosticSeverity.Error));
        }

        [Test]
        public void MissingReferencesCompilationTest()
        {
            var cSharpLang = new CSharpCompilationManager();
            var compilation = cSharpLang.CreateAssemblyCompilation(_simpleCode);

            var assemblyStream = cSharpLang.GetAssemblyStream(compilation, out var diagnostics);

            Assert.AreEqual(assemblyStream, Stream.Null);
            Assert.IsNotNull(diagnostics.FirstOrDefault(d => d.Severity == DiagnosticSeverity.Error));
        }


        [Test]
        public void ProperCodeAndReferencesCompilationTest()
        {
            var cSharpLang = new CSharpCompilationManager();
            var compilation = cSharpLang.CreateAssemblyCompilation(_simpleCode, "CodeGenerationInMemoryAssembly", _referencedAssemblyPaths);

            var assemblyStream = cSharpLang.GetAssemblyStream(compilation, out var diagnostics);

            //Compilation should return an assmebly stream and no diagnostic errors
            Assert.AreNotEqual(assemblyStream, Stream.Null);
            Assert.IsNull(diagnostics.FirstOrDefault(d => d.Severity == DiagnosticSeverity.Error));

            //The ExampleClass object defintition shouldn't be loaded
            var exampleClassType = AppDomain.CurrentDomain.GetAssemblies()
                                                           .Where(a => !a.IsDynamic)
                                                           .SelectMany(a => a.GetTypes().Where(t => t.FullName == "ExampleNS.ExampleClass"))
                                                           .FirstOrDefault();
            Assert.IsNull(exampleClassType);

            //...but after loading it dynamically
            System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(assemblyStream);

            //...it should be there
            exampleClassType = AppDomain.CurrentDomain.GetAssemblies()
                                                      .Where(a => !a.IsDynamic)
                                                      .SelectMany(a => a.GetTypes().Where(t => t.FullName == "ExampleNS.ExampleClass"))
                                                      .FirstOrDefault();
            Assert.IsNotNull(exampleClassType);

            //So we create an object with the default constructor
            var exampleClassInstance = Activator.CreateInstance(exampleClassType);
            var getMessageMethod = exampleClassType.GetMethods().SingleOrDefault(m => m.Name == "GetMessage");
            //...and we should get the default message when calling the GetMessage function
            Assert.AreEqual(getMessageMethod.Invoke(exampleClassInstance, new object[] { }), "Hello World");

            //...in case we instaciate the object with the overloaded constructor we should get our custom message
            exampleClassInstance = Activator.CreateInstance(exampleClassType, "Custom Message");
            Assert.AreEqual(getMessageMethod.Invoke(exampleClassInstance, new object[] { }), "Custom Message");
        }

        [Test]
        public void ComplileAndLoadAssemblyTest()
        {
            var cSharpLang = new CSharpCompilationManager();

            //All in one function
            Assert.IsTrue(cSharpLang.CompileCodeAndLoadAssemblyIntoContext(_clonedCode, _referencedAssemblyPaths));
        }

        [Test]
        public void DuplicateCodeCompilatiion()
        {
            var cSharpLang = new CSharpCompilationManager();
            var compilationSimple = cSharpLang.CreateAssemblyCompilation(_clownedCode, "DuplicateInMemoryAssembly", _referencedAssemblyPaths);
            var assemblyStreamSimple = cSharpLang.GetAssemblyStream(compilationSimple, out var diagnosticsSimple);
            System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(assemblyStreamSimple);

            try
            {
                var compilationSimpleDuplicate = cSharpLang.CreateAssemblyCompilation(_clownedCode, "DuplicateInMemoryAssembly", _referencedAssemblyPaths);
                var assemblyStreamDuplicate = cSharpLang.GetAssemblyStream(compilationSimpleDuplicate, out var diagnosticsCloned);
                System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(assemblyStreamDuplicate);
                Assert.Fail("Duplicate namespace should not be loaded");
            }
            catch
            {

            }

            cSharpLang.CompileCodeAndLoadAssemblyIntoContext(_clownedCode, _referencedAssemblyPaths);


            var exampleClassCloneType = AppDomain.CurrentDomain.GetAssemblies()
                                                           .Where(a => !a.IsDynamic)
                                                           .SelectMany(a => a.GetTypes().Where(t => t.FullName == "ExampleNS.ExampleClassClown"));

            Assert.IsTrue(exampleClassCloneType.Count() == 2);
        }
    }
}
