using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace SokairykFramework.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationRoot _config;
        private IConfigurationProvider _configurationProvider => _config?.Providers?.FirstOrDefault();

        public ConfigurationManager(string jsonFile)
        {
            var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);

            if (!string.IsNullOrWhiteSpace(jsonFile))
                config.AddJsonFile(jsonFile, true, true);
            try
            {
                _config = config.Build();
            }
            catch (Exception ex)
            {
                _config = null;
            }
        }

        public string GetApplicationSetting(string setting)
        {
            string value = null;
            if (_configurationProvider != null)
                _configurationProvider.TryGet(setting, out value);

            return value;
        }
    }
}