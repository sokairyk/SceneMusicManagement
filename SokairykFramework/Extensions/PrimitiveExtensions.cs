using System;
using System.Linq;
using System.Text;

namespace SokairykFramework.Extensions
{
    public static class PrimitiveExtensions
    {
        public static string ToHexString(this string input)
        {
            return string.Join("", input.Select(c => string.Format("{0:X2}", Convert.ToInt32(c))));
        }

        public static string RemoveByteOrderMark(this string input)
        {
            var byteOrderMark = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (input.StartsWith(byteOrderMark))
                input = input.Remove(0, byteOrderMark.Length);

            return input;
        }

        public static TimeSpan ToUnixEpoch(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1));
        }
    }
}
