using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SokairykFramework.AutoMapper
{
    public interface IAutoMapperConfigurator
    {
        void ManualConfiguration(IProfileExpression cfg);
    }
}
