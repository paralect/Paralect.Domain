using NUnit.Framework;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    [TestFixture]
    public class StorageConstructionTest
    {
        [Test]
        public void SimpleTest()
        {
            var repo = new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());

            var storage = new Transitions.TransitionStorage(repo);
        }
    }
}
