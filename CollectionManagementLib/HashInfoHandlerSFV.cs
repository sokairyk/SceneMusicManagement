using CollectionManagementLib.Interfaces;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CollectionManagementLib
{
    public class HashInfoHandlerSFV : IHashInfoHandler
    {
        public string HashInfoExtension => "sfv";
        private const string LINE_VAlIDATION_PATTERN = "([^;\\n\\r]*)( |\\t)+([A-Fa-f0-9]{8}).*";
        private static readonly Regex _lineValidationRegex = new Regex(LINE_VAlIDATION_PATTERN, RegexOptions.Compiled & RegexOptions.Multiline);
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool ValidateFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                _logger.Warn($"Requested SFV info file: {filepath} was not found.");
                return false;
            }

            var contents = File.ReadAllText(filepath);

            return _lineValidationRegex.Match(contents).Success;
        }

        public bool ValidateLine(string line)
        {
            return _lineValidationRegex.Match(line).Success;
        }

        public Dictionary<string, string> Parse(string filepath)
        {
            if (!File.Exists(filepath))
            {
                _logger.Warn($"Requested SFV info file: {filepath} was not found.");
                return null;
            }

            var result = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(filepath))
            {
                var lineValidation = _lineValidationRegex.Match(line);

                if (lineValidation.Success && lineValidation.Length > 3)
                {
                    result.Add(lineValidation.Groups[1].Value, lineValidation.Groups[3].Value);
                }
            }

            return result;
        }
    }
}
