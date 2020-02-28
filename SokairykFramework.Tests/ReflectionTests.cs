using NUnit.Framework;
using SokairykFramework.DynamicCode;
using SokairykFramework.Extensions;
using System;
using System.IO;
using System.Linq;

namespace SokairykFramework.Tests
{
    public class ReflectionTests
    {
        private static readonly string _generatedAssemblyFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReflectionTestsGenerated.dll");

        private static readonly string _simpleCode = @"
using System;
namespace ExampleNS {
    public class SimpleExampleClass {

        private Guid _identifier = Guid.NewGuid();

        public string GetIdentifier()
        {
            return $""{_identifier}"";
        }
    }
}";

        [SetUp]
        public void Setup()
        {
            var cSharpLang = new CSharpCompilationManager();
            var referencedAssemblyPaths = ReflectionExtensions.FindAssembliesFromTypes(new[] { typeof(string), typeof(Guid) });
            var compilation = cSharpLang.CreateAssemblyCompilation(_simpleCode, "ReflectionInMemoryAssembly", referencedAssemblyPaths);
            var assemblyStream = cSharpLang.GetAssemblyStream(compilation, out var diagnostics);

            if (assemblyStream == Stream.Null) Assert.Fail("Code compilation failed. Please review the provided string input");

            try
            {
                if (File.Exists(_generatedAssemblyFilepath)) File.Delete(_generatedAssemblyFilepath);
                using (var fileStream = File.Create(_generatedAssemblyFilepath))
                {
                    assemblyStream.Seek(0, SeekOrigin.Begin);
                    assemblyStream.CopyTo(fileStream);
                    assemblyStream.Close();
                }
            }
            catch
            {
                Assert.Fail($"Could not delete or generate {_generatedAssemblyFilepath}");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            try { File.Delete(_generatedAssemblyFilepath); }
            catch { }
        }

        [Test]
        public void LoadTypeTest()
        {
            //Try to load the generated SimpleExampleClass from the existing assembly context
            //Pass invalid match to avoid the .dll assembly load in the base directory that ReflectionTestsGenerated.dll
            //is generated in the setup process
            var type = ReflectionExtensions.FindTypeInAssemblies(a => a.FullName == "ExampleNS.SimpleExampleClass", filter: "InvalidMatch").SingleOrDefault();

            Assert.IsNull(type);

            //Now load with the proper dll filter
            type = ReflectionExtensions.FindTypeInAssemblies(a => a.FullName == "ExampleNS.SimpleExampleClass", filter: "ReflectionTests").SingleOrDefault();
            Assert.IsNotNull(type);

            var instance = Activator.CreateInstance(type);
            var getIdentifierMethod = type.GetMethods().SingleOrDefault(m => m.Name == "GetIdentifier");

            Assert.IsNotNull(getIdentifierMethod);
            Assert.IsTrue(Guid.TryParse((string)getIdentifierMethod.Invoke(instance, new object[] { }), out var guid));
        }
    }
}
