using log4net;
using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CollectionManagementLib.Helpers
{
    public static class Extensions
    {
        private static Regex _hexValidation = new Regex("^[0-9A-Fa-f]+$", RegexOptions.Compiled & RegexOptions.IgnoreCase);
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string HexToString(this string hexInput)
        {
            if (!_hexValidation.IsMatch(hexInput))
            {
                _logger.Warn("The provided hex input is not a valid hex sequence!");
                return null;
            }

            var raw = new byte[hexInput.Length / 2];

            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hexInput.Substring(i * 2, 2), 16);
            }
            return Encoding.ASCII.GetString(raw);
        }

    }
}
