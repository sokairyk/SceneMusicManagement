using AutoMapper;

namespace Sokairyk.AutoMapper
{
    public interface IAutoMapperConfigurator
    {
        void ManualConfiguration(IProfileExpression cfg);
    }
}
