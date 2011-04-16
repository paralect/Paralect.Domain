using System;
using NUnit.Framework;
using Paralect.Transitions.Mongo.Test.Events;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    [TestFixture]
    public class SimpleTest
    {
        public StreamCreatedEvent CreatedStreamCreatedEvent()
        {
            return new StreamCreatedEvent()
            {
                Name = "New Stream",
                Number = 678,
                Type = "Created"
            };
        }

        [Test]
        public void ShouldSaveOneTransition()
        {
            var streamId = Guid.NewGuid().ToString();
            var repository = Helper.GetRepository();
            var storage = new Transitions.TransitionStorage(repository);
            var stream = storage.CreateStream(streamId);

            StreamCreatedEvent domainEvent = CreatedStreamCreatedEvent();

            var transitionEvent = new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, domainEvent, null);

            stream.AddEvent(transitionEvent);
            stream.Commit();

            var transitions = repository.GetTransitions(streamId, 0, int.MaxValue);
            
            Assert.AreEqual(transitions.Count, 1);
            Assert.AreEqual(transitions[0].Id.StreamId, streamId);
            Assert.AreEqual(transitions[0].Id.Version, 1);
            Assert.AreEqual(transitions[0].Events.Count, 1);
            Assert.AreNotEqual(transitions[0].Events[0].Data, null);
            Assert.AreEqual(transitions[0].Events[0].Data.GetType(), typeof(StreamCreatedEvent));

            var storedEvent = transitions[0].Events[0].Data as StreamCreatedEvent;

            Assert.AreEqual(storedEvent.Name, domainEvent.Name);
            Assert.AreEqual(storedEvent.Number, domainEvent.Number);
            Assert.AreEqual(storedEvent.Type, domainEvent.Type);

            repository.RemoveStream(streamId);
        }

        [Test]
        public void ShouldThrowOnTheSecondCommit()
        {
            var streamId = Guid.NewGuid().ToString();

            var repository = Helper.GetRepository();
            var storage = new Transitions.TransitionStorage(repository);
            var stream = storage.CreateStream(streamId);

            StreamCreatedEvent domainEvent = CreatedStreamCreatedEvent();

            var transitionEvent = new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, domainEvent, null);

            stream.AddEvent(transitionEvent);
            stream.Commit();

            // Should throw on second commit
            Assert.Throws<ConcurrencyException>(stream.Commit);

            repository.RemoveStream(streamId);
        }

        [Test]
        public void ShouldSaveOneTransitionWithThreeEvents()
        {
            var streamId = Guid.NewGuid().ToString();
            var repository = Helper.GetRepository();
            var storage = new Transitions.TransitionStorage(repository);
            var stream = storage.CreateStream(streamId);

            StreamCreatedEvent domainEvent1 = CreatedStreamCreatedEvent();
            StreamCreatedEvent domainEvent2 = CreatedStreamCreatedEvent();
            StreamCreatedEvent domainEvent3 = CreatedStreamCreatedEvent();

            domainEvent1.Number = 1;
            domainEvent2.Number = 2;
            domainEvent3.Number = 3;

            var transitionEvent1 = new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, domainEvent1, null);
            var transitionEvent2 = new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, domainEvent2, null);
            var transitionEvent3 = new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, domainEvent3, null);

            stream.AddEvent(transitionEvent1);
            stream.AddEvent(transitionEvent2);
            stream.AddEvent(transitionEvent3);
            stream.Commit();

            var transitions = repository.GetTransitions(streamId, 0, int.MaxValue);

            Assert.AreEqual(transitions.Count, 1);
            Assert.AreEqual(transitions[0].Id.StreamId, streamId);
            Assert.AreEqual(transitions[0].Id.Version, 1);
            Assert.AreEqual(transitions[0].Events.Count, 3);
            Assert.AreNotEqual(transitions[0].Events[0].Data, null);
            Assert.AreEqual(transitions[0].Events[0].Data.GetType(), typeof(StreamCreatedEvent));
            Assert.AreEqual(transitions[0].Events[1].Data.GetType(), typeof(StreamCreatedEvent));
            Assert.AreEqual(transitions[0].Events[2].Data.GetType(), typeof(StreamCreatedEvent));

            var storedEvent1 = transitions[0].Events[0].Data as StreamCreatedEvent;
            var storedEvent2 = transitions[0].Events[1].Data as StreamCreatedEvent;
            var storedEvent3 = transitions[0].Events[2].Data as StreamCreatedEvent;

            Assert.AreEqual(storedEvent1.Number, domainEvent1.Number);
            Assert.AreEqual(storedEvent2.Number, domainEvent2.Number);
            Assert.AreEqual(storedEvent3.Number, domainEvent3.Number);

            repository.RemoveStream(streamId);
        }

        public void ShouldSaveThreeTransitionsForOneStream()
        {
            
        }
    }
}