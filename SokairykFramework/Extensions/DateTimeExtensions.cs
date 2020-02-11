using System;

namespace SokairykFramework.Extensions
{
    public static class DateTimeExtensions
    {
        public static TimeSpan ToUnixEpoch(this DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1));
        }
    }
}
