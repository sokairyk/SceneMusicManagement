namespace Sokairyk.AutoMapper.Tests.DataModel
{
    class TestEntityA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestEntityC ComplexProp { get; set; }
    }

    class TestEntityB
    {
        public int Number { get; set; }
        public string Username { get; set; }

        public Guid Code { get; set; }
        public double CodePrice { get; set; }
    }

    class TestEntityC
    {
        public Guid Code { get; set; }
        public string Item { get; set; }
        public decimal Value { get; set; }
    }
}
