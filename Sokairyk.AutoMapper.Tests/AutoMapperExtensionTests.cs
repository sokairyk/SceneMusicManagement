using AutoMapper;
using Sokairyk.AutoMapper.Tests.DataModel;

namespace Sokairyk.AutoMapper.Tests
{
    public class AutoMapperExtensionTests
    {
        MapperConfiguration _config;
        IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _config = AutoMapperExtensions.CreateConfig();
            _mapper = _config.CreateMapper();
        }

        [Test]
        public void AutoMapperConversionTests()
        {
            var entityA = new TestEntityA { Id = 7, Name = "Testing...", ComplexProp = new TestEntityC { Code = Guid.NewGuid(), Item = "Something", Value = 54.36m } };
            var entityB = _mapper.Map<TestEntityB>(entityA);

            Assert.IsTrue(entityA?.Id == entityB?.Number);
            Assert.IsTrue(entityA?.Name == entityB?.Username);
            Assert.IsTrue(entityA?.ComplexProp.Code == entityB?.Code);
            Assert.IsTrue(entityA?.ComplexProp.Value == (decimal)entityB?.CodePrice);

            entityB = new TestEntityB { Number = 15, Username = "Someone", Code = Guid.NewGuid(), CodePrice = 45.66f };
            entityA = _mapper.Map<TestEntityA>(entityB);

            Assert.IsTrue(entityA?.Id == entityB?.Number);
            Assert.IsTrue(entityA?.Name == entityB?.Username);
            Assert.IsTrue(entityA?.ComplexProp.Code == entityB?.Code);
            Assert.IsTrue(entityA?.ComplexProp.Value == (decimal)entityB?.CodePrice);
        }

    }
}
