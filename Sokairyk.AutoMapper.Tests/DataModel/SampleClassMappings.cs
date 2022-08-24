using AutoMapper;

namespace Sokairyk.AutoMapper.Tests.DataModel
{
    class TestEntityA2BMapping : IAutoMapperConfigurator
    {
        public void ManualConfiguration(IProfileExpression cfg)
        {
            cfg.CreateMappings((TestEntityA x) => new TestEntityB
            {
                Number = x.Id,
                Username = x.Name,
                Code = x.ComplexProp != null ? x.ComplexProp.Code : Guid.Empty,
                CodePrice = x.ComplexProp != null ? (double)x.ComplexProp.Value : default
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
}
