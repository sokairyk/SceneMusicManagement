using log4net;
using log4net.Config;
using SokairykFramework.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SokairykFramework.Logger
{
    public sealed class Log4NetLogger : ILogger
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<Log4NetLogger> _instance = new Lazy<Log4NetLogger>(() => new Log4NetLogger());
        public static Log4NetLogger Instance { get => _instance.Value; }

        private Log4NetLogger()
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
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SokairykFramework.Logger.log4net.config.default"))
                    using (var reader = new StreamReader(stream))
                    {
                        var defaultConfiguration = reader.ReadToEnd();
                        //Save the default configuration file for next run
                        File.WriteAllText(logConfiguration, defaultConfiguration);
                        //Load the default configuration
                        var log4netDefaultConfig = GetXmlElement(defaultConfiguration.RemoveByteOrderMark());
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

        public void LogError(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
    }
}

