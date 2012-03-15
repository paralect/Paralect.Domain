using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paralect.Transitions.Mongo.Test.Events;

namespace Paralect.Transitions.Mongo.Test.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        [Test]
        public void ShouldSaveThreeEventsTest()
        {
            var streamId = Guid.NewGuid().ToString();

            var repository = new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());

            var events = new List<TransitionEvent> {
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition> {
                new Transition(new TransitionId(streamId, 1), DateTime.UtcNow, events, null)
            };

            repository.SaveTransition(transitions[0]);

            var storedTransitions = repository.GetTransitions(streamId, 0, int.MaxValue);
            TransitionAsserter.AssertTransitions(transitions, storedTransitions);

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
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition>{
                new Transition(new TransitionId(streamId, 1), DateTime.UtcNow, events, null),
                new Transition(new TransitionId(streamId, 2), DateTime.UtcNow, events, null),
                new Transition(new TransitionId(streamId, 3), DateTime.UtcNow, events, null)
            };

            repository.SaveTransition(transitions[0]);
            repository.SaveTransition(transitions[1]);
            repository.SaveTransition(transitions[2]);

            var storedTransitions = repository.GetTransitions(streamId, 0, int.MaxValue);
            TransitionAsserter.AssertTransitions(transitions, storedTransitions);

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
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition>{
                new Transition(new TransitionId(streamId, 1), DateTime.UtcNow, events, null),
                new Transition(new TransitionId(streamId, 2), DateTime.UtcNow, events, null),
                new Transition(new TransitionId(streamId, 3), DateTime.UtcNow, events, null)
            };

            repository.SaveTransition(transitions[0]);
            repository.SaveTransition(transitions[1]);
            repository.SaveTransition(transitions[2]);

            #region search within defined stream

            var storedTransitions = repository.GetTransitions(streamId, 1, 3);
            TransitionAsserter.AssertTransitions(transitions, storedTransitions);

            var storedTransitions2 = repository.GetTransitions(streamId, 1, 1);
            TransitionAsserter.AssertTransitions(new List<Transition> { transitions[0] }, storedTransitions2);

            var storedTransitions3 = repository.GetTransitions(streamId, 2, 2);
            TransitionAsserter.AssertTransitions(new List<Transition> { transitions[1] }, storedTransitions3);

            var storedTransitions4 = repository.GetTransitions(streamId, 2, 3);
            TransitionAsserter.AssertTransitions(new List<Transition> { transitions[1], transitions[2] }, storedTransitions4);

            #endregion

            #region search everywhere

            var storedTransitions5 = repository.GetTransitions(0, 3);
            TransitionAsserter.AssertTransitions(transitions, storedTransitions5);

            var storedTransitions6 = repository.GetTransitions(1, 1);
            TransitionAsserter.AssertTransitions(new List<Transition> { transitions[1] }, storedTransitions6);

            var storedTransitions7 = repository.GetTransitions(1, 2);
            TransitionAsserter.AssertTransitions(new List<Transition> { transitions[1], transitions[2] }, storedTransitions7);

            var storedTransitions8 = repository.GetTransitions(2, 100);
            TransitionAsserter.AssertTransitions(new List<Transition> { transitions[2] }, storedTransitions8);

            var storedTransitions9 = repository.GetTransitions(3, 1);
            Assert.IsEmpty(storedTransitions9);

            #endregion

            repository.RemoveStream(streamId);            
        }

        [Test]
        public void ShouldThrowIfSuchVersionExists()
        {
            var streamId = Guid.NewGuid().ToString();

            var repository = new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                Helper.GetConnectionString());

            var events = new List<TransitionEvent> {
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(1), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(2), null), 
                new TransitionEvent(typeof(StreamCreatedEvent).AssemblyQualifiedName, TransitionAsserter.CreatedStreamCreatedEvent(3), null)
            };

            var transitions = new List<Transition> {
                new Transition(new TransitionId(streamId, 1), DateTime.UtcNow, events, null), 
                new Transition(new TransitionId(streamId, 1), DateTime.UtcNow, events, null) // Already existed version specified!
            };

            repository.SaveTransition(transitions[0]);
            
            Assert.Throws<ConcurrencyException>(() => repository.SaveTransition(transitions[0]));
            
            repository.RemoveStream(streamId);
        }
    }
}
