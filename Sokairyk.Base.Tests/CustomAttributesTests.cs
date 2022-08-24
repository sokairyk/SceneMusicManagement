using Sokairyk.Base.CustomAttributes;
using System.Reflection;

namespace Sokairyk.Base.Tests
{
    internal enum ValidEnumAttributes
    {
        [StringValue("valid-1")]
        Value1,
        [StringValue("valid-2")]
        Value2,
        [StringValue("valid-3")]
        Value3
    }

    internal enum InValidEnumAttributes
    {
        [StringValue("invalid")]
        Value1,
        [StringValue("invalid")]
        Value2,
        Value3
    }

    public class CustomAttributesTests
    {
        [Test]
        public void StringValueTest()
        {
            Assert.That(ValidEnumAttributes.Value1.StringValue(), Is.EqualTo("valid-1"));
            Assert.That(InValidEnumAttributes.Value1.StringValue(), Is.EqualTo("invalid"));
            Assert.That(InValidEnumAttributes.Value3.StringValue(), Is.Not.EqualTo("invalid"));

            Assert.That(ValidEnumAttributes.Value1, Is.EqualTo("valid-1".ParseFromStringValue<ValidEnumAttributes>()));

            try
            {
                var enumValue = "valid-7".ParseFromStringValue<ValidEnumAttributes>();
                Assert.Fail("Should have raised an exception");
            }
            catch (KeyNotFoundException ex)
            {
                //Should go here
            }
            catch (Exception)
            {
                Assert.Fail("Should be a key not found exception.");
            }

            try
            {
                var enumValue = "invalid".ParseFromStringValue<InValidEnumAttributes>();
                Assert.Fail("Should have raised an exception");
            }
            catch (AmbiguousMatchException ex)
            {
                //Should go here
            }
            catch (Exception)
            {
                Assert.Fail("Should be a key not found exception.");
            }
        }
    }
}
