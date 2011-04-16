using System;
using NUnit.Framework;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    public class SimpleType
    {
        public String Name { get; set; }
        public Int32 Value { get; set; }
        public Boolean Flag { get; set; }
        public DateTime DateTime { get; set; }
        public InnerClass Inner { get; set; }

        public class InnerClass
        {
            public String InnerValue { get; set; }
        }
    }

    [TestFixture]
    public class DataSerializerTest
    {
        public void ShouldSerializeSimpleType()
        {
            var obj = new SimpleType
            {
                DateTime = DateTime.UtcNow,
                Flag = true,
                Name = "Name",
                Value = 456,
                Inner = new SimpleType.InnerClass()
                {
                    InnerValue = "Inner"
                }
            };

            var serializer = new MongoTransitionDataSerializer(new AssemblyQualifiedDataTypeRegistry());
            var doc = serializer.Serialize(obj);

            var back = (SimpleType) serializer.Deserialize(doc, typeof(SimpleType));

            Assert.AreEqual(obj.Name, back.Name);
            Assert.AreEqual(obj.Flag, back.Flag);
            Assert.AreEqual(obj.Inner.InnerValue, back.Inner.InnerValue);
            Assert.AreEqual(obj.DateTime, back.DateTime);
        }
    }
}
