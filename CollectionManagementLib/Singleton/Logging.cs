using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CollectionManagementLib
{
    public sealed class Logging
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ILog Logger { get => _logger; }

        private static readonly Lazy<Logging> _instance = new Lazy<Logging>(() => new Logging());
        public static Logging Instance { get => _instance.Value; }

        public static bool UseFallbackConfiguration { get; set; }

        private Logging()
        {
            //Get log4net repository for current assembly
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            try
            {
                //Ensure that a configuration exists
                var logConfiguration = Path.Combine(Environment.CurrentDirectory, "log4net.config");
                if (File.Exists(logConfiguration))
                {
                    XmlConfigurator.Configure(logRepository, new FileInfo(logConfiguration));
                }
                else
                {
                    //...else try to load the default configuration from the embeded resource
                    if (!UseFallbackConfiguration)
                        throw new FileNotFoundException("ERROR: log4net.config was not found in execution folder!");

                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("log4net.config.default"))
                    using (var reader = new StreamReader(stream))
                    {
                        var log4netDefaultConfig = GetXmlElement(reader.ReadToEnd());
                        XmlConfigurator.Configure(logRepository, log4netDefaultConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: Couldn't load log4net.config file properly!", ex);
            }
        }

        private static XmlElement GetXmlElement(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}
