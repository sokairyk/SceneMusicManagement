namespace Sokairyk.Repository.NHibernate.Tests.DataModel
{
    public class TestEntity
    {
        public virtual int Id { get; set; }
        public virtual string TextField { get; set; }
        public virtual bool IsBoolField { get; set; }
        public virtual decimal PrecisionField { get; set; }
    }
}
