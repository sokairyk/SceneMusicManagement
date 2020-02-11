using System;
using System.Collections.Generic;
using System.Text;

namespace SokairykFramework.Configuration
{
    public interface IConfigurationManager
    {
        string GetApplicationSetting(string setting);
    }
}
