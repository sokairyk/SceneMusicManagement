using NUnit.Framework;
using SokairykFramework.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace SokairykFramework.Tests
{
    public class ConfigurationTests
    {
        private const string APP_SETTINGS_FILENAME = "appsettings.json";
        private const string INVALID_APP_SETTINGS_FILENAME = "invalid.appsettings.json";
        private const string RELOADED_APP_SETTINGS_FILENAME = "reloaded.appsettings.json";
        private string _appSettingsPath = Path.Combine(AppContext.BaseDirectory, APP_SETTINGS_FILENAME);
        private string _invalidAppSettingsPath = Path.Combine(AppContext.BaseDirectory, INVALID_APP_SETTINGS_FILENAME);
        private string _reloadedAppSettingsPath = Path.Combine(AppContext.BaseDirectory, RELOADED_APP_SETTINGS_FILENAME);

        private void DeleteSettingsFiles()
        {
            File.Delete(_appSettingsPath);
            File.Delete(_invalidAppSettingsPath);
            File.Delete(_reloadedAppSettingsPath);
        }

        private void CreateSettingsFiles()
        {
            var jsonSettings = @"
{
    ""test-setting"": ""setting-value"",
    ""test-array-setting"": [ ""hello"", 2, { ""id"" : 44 } ],
    ""test-object-setting"": { ""id"" : 44 }
}";
            var jsonInvalidSettings = @"{ This is not a valid = :json file { ; tis super messed up [0], 2 } ";

            File.WriteAllText(_appSettingsPath, jsonSettings);
            File.WriteAllText(_invalidAppSettingsPath, jsonInvalidSettings);
            File.Copy(_appSettingsPath, _reloadedAppSettingsPath);

        }


        [SetUp]
        public void Setup()
        {
            try
            {
                DeleteSettingsFiles();
                CreateSettingsFiles();
            }
            catch
            {
                Assert.Fail($"Could not delete/create the AppSettings files in {AppContext.BaseDirectory}");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            try { DeleteSettingsFiles(); }
            catch { }
        }

        [Test]
        public void AppSettingsFileMissingTest()
        {
            var configurationManager = new ConfigurationManager(null);

            Assert.IsNull(configurationManager.GetApplicationSetting("test"));
        }

        [Test]
        public void AppSettingsFileExistingTest()
        {
            var configurationManager = new ConfigurationManager(APP_SETTINGS_FILENAME);

            Assert.IsNull(configurationManager.GetApplicationSetting("test"));
            Assert.IsNull(configurationManager.GetApplicationSetting("test-array-setting"));
            Assert.IsNull(configurationManager.GetApplicationSetting("test-object-setting"));
        }

        [Test]
        public void AppSettingsFileInvalidTest()
        {
            var configurationManager = new ConfigurationManager(INVALID_APP_SETTINGS_FILENAME);

            Assert.IsNull(configurationManager.GetApplicationSetting("test"));
            Assert.IsNull(configurationManager.GetApplicationSetting("test-setting"));
        }

        [Test]
        public void AppSettingsFileReloadTest()
        {
            var configurationManager = new ConfigurationManager(RELOADED_APP_SETTINGS_FILENAME);

            Assert.AreEqual("setting-value", configurationManager.GetApplicationSetting("test-setting"));
            Assert.IsNull(configurationManager.GetApplicationSetting("test"));

            try
            {
                var lineSettings = new List<string>();
                using (var sr = File.OpenText(_reloadedAppSettingsPath))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        if (line.Trim().StartsWith("\"test-object-setting\""))
                            line = @"""test"": ""new-setting!""";

                        lineSettings.Add(line);
                    }
                }

                using (var sw = new StreamWriter(_reloadedAppSettingsPath))
                {
                    for (var i = 0; i < lineSettings.Count; i++)
                        sw.WriteLine(lineSettings[i]);
                }
            }
            catch
            {
                Assert.Fail($"Could not update the AppSettings file in {AppContext.BaseDirectory}");
            }

            //Give some time to allow reload on change
            System.Threading.Thread.Sleep(500);

            Assert.AreEqual("new-setting!", configurationManager.GetApplicationSetting("test"));
        }
    }
}