using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Paralect.Transitions.Mongo.Test.Events;

namespace Paralect.Transitions.Mongo.Test
{
    public class TransitionAsserter
    {
        public static StreamCreatedEvent CreatedStreamCreatedEvent(Int32 number)
        {
            return new StreamCreatedEvent()
            {
                Name = "New Stream",
                Number = number,
                Type = "Created"
            };
        }

        public static void AssertEvent(Object original, Object stored)
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

        public static void AssertTransition(Transition original, Transition stored)
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

        public static void AssertTransitions(List<Transition> originalTransitions, List<Transition> storedTransitions)
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
    }
}
