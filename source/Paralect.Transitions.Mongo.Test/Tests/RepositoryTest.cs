using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paralect.Transitions.Mongo.Test.Events;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        private StreamCreatedEvent CreatedStreamCreatedEvent(Int32 number)
        {
            return new StreamCreatedEvent()
            {
                Name = "New Stream",
                Number = number,
                Type = "Created"
            };
        }

        private void AssertEvent(Object original, Object stored)
        {
            Assert.AreNotEqual(original, stored);
            if (original is StreamCreatedEvent)
            {
                var createdOriginal = original as StreamCreatedEvent;
                var createdStored = stored as StreamCreatedEvent;

                Assert.AreEqual(createdOriginal.Number, createdStored.Number);
                Assert.AreEqual(createdOriginal.Name, createdStored.Name);
                Assert.AreEqual(createdOriginal.Type, createdStored.Type);
            }
        }

        private void AssertTransition(Transition original, Transition stored)
        {
            Assert.AreNotEqual(original, stored);
            Assert.AreEqual(original.Id.StreamId, stored.Id.StreamId);
            Assert.AreEqual(original.Id.Version, stored.Id.Version);
            Assert.AreEqual(original.Events.Count, stored.Events.Count);

            for (int i = 0; i < original.Events.Count; i++)
            {
                var originalEvent = original.Events[i];
                var storedEvent = stored.Events[i];

                Assert.AreNotEqual(storedEvent.Data, null);
                Assert.AreEqual(originalEvent.Data.GetType(), storedEvent.Data.GetType());
                AssertEvent(originalEvent.Data, storedEvent.Data);
            }
        }

        private void AssertTransitions(List<Transition> originalTransitions, List<Transition> storedTransitions)
        {
            Assert.AreNotEqual(originalTransitions, storedTransitions);
            Assert.AreEqual(originalTransitions.Count, storedTransitions.Count);

            for (int i = 0; i < originalTransitions.Count; i++)
            {
                var original = originalTransitions[i];
                var stored = storedTransitions[i];

                AssertTransition(original, stored);
            }
        }

        [Test]
        public void ShouldSaveThreeEventsTest()
        {
            var streamId = Guid.NewGuid().ToString();

            var repository = new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());

            var events = new List<TransitionEvent> {
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition> {
                new Transition(new TransitionId(streamId, 1), events, null)
            };

            repository.SaveTransition(transitions[0]);

            var storedTransitions = repository.GetTransitions(streamId, 0, int.MaxValue);

            AssertTransitions(transitions, storedTransitions);

            repository.RemoveStream(streamId);            
        }

        [Test]
        public void ShouldSaveThreeTransitionsTest()
        {
            var streamId = Guid.NewGuid().ToString();

            var repository = new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());

            var events = new List<TransitionEvent>{
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition>{
                new Transition(new TransitionId(streamId, 1), events, null),
                new Transition(new TransitionId(streamId, 2), events, null),
                new Transition(new TransitionId(streamId, 3), events, null)
            };

            repository.SaveTransition(transitions[0]);
            repository.SaveTransition(transitions[1]);
            repository.SaveTransition(transitions[2]);

            var storedTransitions = repository.GetTransitions(streamId, 0, int.MaxValue);

            AssertTransitions(transitions, storedTransitions);

            repository.RemoveStream(streamId);            
        }

        [Test]
        public void ShouldCorrectlyUnderstandPagingTest()
        {
            var streamId = Guid.NewGuid().ToString();

            var repository = new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());

            var events = new List<TransitionEvent>{
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition>{
                new Transition(new TransitionId(streamId, 1), events, null),
                new Transition(new TransitionId(streamId, 2), events, null),
                new Transition(new TransitionId(streamId, 3), events, null)
            };

            repository.SaveTransition(transitions[0]);
            repository.SaveTransition(transitions[1]);
            repository.SaveTransition(transitions[2]);

            var storedTransitions = repository.GetTransitions(streamId, 1, 3);
            AssertTransitions(transitions, storedTransitions);

            var storedTransitions2 = repository.GetTransitions(streamId, 1, 1);
            AssertTransitions(new List<Transition> { transitions[0] }, storedTransitions2);

            var storedTransitions3 = repository.GetTransitions(streamId, 2, 2);
            AssertTransitions(new List<Transition> { transitions[1] }, storedTransitions3);

            var storedTransitions4 = repository.GetTransitions(streamId, 2, 3);
            AssertTransitions(new List<Transition> { transitions[1], transitions[2] }, storedTransitions4);

            repository.RemoveStream(streamId);            
        }
    }
}
