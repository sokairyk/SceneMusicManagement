using System.Reflection;

namespace Sokairyk.Base.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StringValueAttribute : Attribute
    {
        private readonly string _value;
        public string Value => _value;
        public StringValueAttribute(string value)
        {
            _value = value;
        }
    }

    public static class CustomAttributeExtensions
    {
        public static string StringValue(this Enum value)
        {
            var stringValueAttribute = value.GetType()
                                            .GetField(value.ToString())
                                            ?.GetCustomAttribute<StringValueAttribute>();

            return stringValueAttribute?.Value;
        }

        public static T ParseFromStringValue<T>(this string value) where T : Enum
        {
            var fieldsInfo = typeof(T).GetFields().Where(f => f.GetCustomAttribute<StringValueAttribute>()?.Value == value);

            if (fieldsInfo.Count() > 1) throw new AmbiguousMatchException($"Multiple StringValue {value} on enum {typeof(T).Name}! Please correct and retry.");
            if (fieldsInfo.Count() == 0) throw new KeyNotFoundException($"No StringValue {value} found on enum {typeof(T).Name}!");
            return (T)Enum.Parse(typeof(T), fieldsInfo.FirstOrDefault().Name);
        }
    }
}
