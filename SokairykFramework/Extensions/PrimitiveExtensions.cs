using System;
using System.Linq;

namespace SokairykFramework.Extensions
{
    public static class PrimitiveExtensions
    {
        public static string ToHexString(this string input)
        {
            return string.Join("", input.Select(c => string.Format("{0:X2}", Convert.ToInt32(c))));
        }

        public static TimeSpan ToUnixEpoch(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1));
        }
    }
}
