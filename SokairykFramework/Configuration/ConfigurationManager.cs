using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace SokairykFramework.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private static readonly IConfigurationRoot _config;
        private static IConfigurationProvider _configurationProvider => _config.Providers.FirstOrDefault();

        static ConfigurationManager()
        {
            _config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                                                .AddJsonFile("appsettings.json", true, true)
                                                .Build();
        }

        public string GetApplicationSetting(string setting)
        {
            _configurationProvider.TryGet(setting, out var value);
            return value;
        }
    }
}