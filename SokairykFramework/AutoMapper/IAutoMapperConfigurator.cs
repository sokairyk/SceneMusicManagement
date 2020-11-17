using AutoMapper;

namespace SokairykFramework.AutoMapper
{
    public interface IAutoMapperConfigurator
    {
        void ManualConfiguration(IProfileExpression cfg);
    }
}