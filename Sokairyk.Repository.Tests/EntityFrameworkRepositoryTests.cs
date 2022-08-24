namespace Sokairyk.Repository.Tests
{
    public class EntityFrameworkRepositoryTests
    {
        private static readonly string _tempDBPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.db");

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Cleanup()
        {
            try
            {
                File.Delete(_tempDBPath);
            }
            catch
            {
            }
        }


    }
}
