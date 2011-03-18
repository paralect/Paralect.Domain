using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Domain.Test.Events;
using MongoDB.Driver;
using NUnit.Framework;

namespace Paralect.Domain.Test.Tests
{
    [TestFixture]
    public class MongoTest
    {
        [Test]
        public void DoIt()
        {
            MongoUrlBuilder builder = new MongoUrlBuilder("mongodb://localhost:27018");

        }

        [Test]
        public void Zzzz()
        {

        }
    }
}
