using NHibernate.Mapping.ByCode.Conformist;

namespace Sokairyk.Repository.NHibernate.Tests.DataModel
{
    public class TestEntityMapping : ClassMapping<TestEntity>
    {
        public TestEntityMapping()
        {
            Table("entities");
            Lazy(true);
            Id(x => x.Id);
            Property(x => x.TextField);
            Property(x => x.IsBoolField, map => map.NotNullable(true));
            Property(x => x.PrecisionField, map => map.Column("presicion_field"));
        }
    }
}
