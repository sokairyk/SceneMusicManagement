using NHibernate.Mapping.ByCode.Conformist;

namespace SokairykFramework.Tests.Repository
{
    public class TestEntity
    {
        public virtual int Id { get; set; }
        public virtual string TextField { get; set; }
        public virtual bool IsBoolField { get; set; }
        public virtual decimal PrecisionField { get; set; }
    }

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
