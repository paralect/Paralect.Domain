using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Paralect.Transitions.Mongo.Test.Events;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    [TestFixture]
    public class StreamTest
    {
        public Transition CreatedTransition(String streamId, Int32 version)
        {
            var domainEvent = new StreamCreatedEvent()
            {
                Name = "New Stream",
                Number = 678,
                Type = "Created"
            };

            var transitionEvent = new TransitionEvent(
                typeof(StreamCreatedEvent).AssemblyQualifiedName, 
                domainEvent, 
                null);

            return new Transition(
                new TransitionId(streamId, version), 
                new List<TransitionEvent> { transitionEvent }, 
                null);
        }

        public ITransitionRepository GetRepository()
        {
            return new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());            
        }

        [Test]
        public void SimpleStorageTest()
        {
            var streamId = Guid.NewGuid().ToString();
            var storage = new TransitionStorage(GetRepository());
            var originalTransitions = new List<Transition> {
                CreatedTransition(streamId, 1),
                CreatedTransition(streamId, 2),
                CreatedTransition(streamId, 3)
            };

            using (var stream = storage.OpenStream(streamId))
            {
                stream.Write(originalTransitions[0]);
                stream.Write(originalTransitions[1]);
                stream.Write(originalTransitions[2]);
            }

            List<Transition> storedTransitions;
            using (var stream = storage.OpenStream(streamId))
            {
                storedTransitions = stream.Read().ToList();
            }

            TransitionAsserter.AssertTransitions(originalTransitions, storedTransitions);

            GetRepository().RemoveStream(streamId);
        }

        [Test]
        public void ShouldThrowOnDuplication()
        {
            var streamId = Guid.NewGuid().ToString();
            var storage = new TransitionStorage(GetRepository());
            var originalTransitions = new List<Transition> {
                CreatedTransition(streamId, 1),
                CreatedTransition(streamId, 1),
                CreatedTransition(streamId, 1)
            };

            using (var stream = storage.OpenStream(streamId))
            {
                stream.Write(originalTransitions[0]);
                Assert.Throws<ConcurrencyException>(()=> stream.Write(originalTransitions[1]));
                Assert.Throws<ConcurrencyException>(() => stream.Write(originalTransitions[2]));
            }

            GetRepository().RemoveStream(streamId);
        }

        [Test]
        public void ShouldNotAllowWritingWhenInReadMode()
        {
            var streamId = Guid.NewGuid().ToString();
            var storage = new TransitionStorage(GetRepository());
            var originalTransitions = new List<Transition> {
                CreatedTransition(streamId, 1),
            };

            using (var stream = storage.OpenStream(streamId))
            {
                stream.Write(originalTransitions[0]);
            }

            using (var stream = storage.OpenStream(streamId))
            {
                stream.Read().ToList();
                Assert.Throws<InvalidOperationException>(() => stream.Write(originalTransitions[0]));
            }

            GetRepository().RemoveStream(streamId);            
        }
    }
}