using AutoMapper;
using NUnit.Framework;
using SokairykFramework.AutoMapper;
using System;

namespace SokairykFramework.Tests
{
    public class AutoMapperTests
    {
        class TestEntityA
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public TestEntityC ComplexProp { get; set; }
        }

        class TestEntityB
        {
            public int Number { get; set; }
            public string Username { get; set; }

            public Guid Code { get; set; }
            public double CodePrice { get; set; }
        }

        class TestEntityC
        {
            public Guid Code { get; set; }
            public string Item { get; set; }
            public decimal Value { get; set; }
        }

        class TestEntityA2BMapping : IAutoMapperConfigurator
        {
            public void ManualConfiguration(IProfileExpression cfg)
            {
                cfg.CreateMappings((TestEntityA x) => new TestEntityB
                {
                    Number = x.Id,
                    Username = x.Name,
                    Code = x.ComplexProp != null ? x.ComplexProp.Code : Guid.Empty,
                    CodePrice = x.ComplexProp != null ? (double)x.ComplexProp.Value : default(double)
                });
            }
        }

        class TestEntityB2AMapping : IAutoMapperConfigurator
        {
            public void ManualConfiguration(IProfileExpression cfg)
            {
                cfg.CreateMappings((TestEntityB x) => new TestEntityA
                {
                    Id = x.Number,
                    Name = x.Username,
                    ComplexProp = new TestEntityC
                    {
                        Code = x.Code,
                        Value = (decimal)x.CodePrice
                    }
                });
            }
        }

        [Test]
        public void AutoMapperConversionTest()
        {
            var config = AutoMapperExtensions.CreateConfig();
            var mapper = config.CreateMapper();

            var entityA = new TestEntityA { Id = 7, Name = "Testing...", ComplexProp = new TestEntityC { Code = Guid.NewGuid(), Item = "Something", Value = 54.36m } };
            var entityB = mapper.Map<TestEntityB>(entityA);

            Assert.IsTrue(entityA?.Id == entityB?.Number);
            Assert.IsTrue(entityA?.Name == entityB?.Username);
            Assert.IsTrue(entityA?.ComplexProp.Code == entityB?.Code);
            Assert.IsTrue(entityA?.ComplexProp.Value == (decimal)entityB?.CodePrice);

            entityB = new TestEntityB { Number = 15, Username = "Someone", Code = Guid.NewGuid(), CodePrice = 45.66f };
            entityA = mapper.Map<TestEntityA>(entityB);

            Assert.IsTrue(entityA?.Id == entityB?.Number);
            Assert.IsTrue(entityA?.Name == entityB?.Username);
            Assert.IsTrue(entityA?.ComplexProp.Code == entityB?.Code);
            Assert.IsTrue(entityA?.ComplexProp.Value == (decimal)entityB?.CodePrice);
        }

    }
}
