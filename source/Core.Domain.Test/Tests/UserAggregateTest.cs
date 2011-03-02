using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.Domain.Test.Aggregates;
using Core.Domain.Test.Cleanupers;
using Core.Domain.Test.Events;
using Core.Domain.Utilities;
using NUnit.Framework;
using System.Diagnostics;

namespace Core.Domain.Test.Tests
{
    [TestFixture]
    public class UserAggregateTest
    {
        [Test]
        public void SimpleTest()
        {
            var userId = Guid.NewGuid().ToString();

            var event1 = new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            };

            var event2 = new UserNameChangedEvent()
            {
                UserId = userId,
                FirstName = "John Jr.",
                LastName = "Melinda III"
            };

            var changeset1 = new Changeset(userId, 1);
            changeset1.AddEvent(event1);
            changeset1.AddEvent(event2);

            var event3 = new UserEmailChangedEvent()
            {
                UserId = userId,
                Email = "john.jr@test.com",
            };

            var changeset2 = new Changeset(userId, 2);
            changeset2.AddEvent(event3);

            using (new AggregateCleanuper(Helper.GetEventServer(), changeset1.AggregateId)) 
            using (new AggregateCleanuper(Helper.GetEventServer(), changeset2.AggregateId))
            {
                var store = Helper.GetEventStore();
                store.SaveChangeset(changeset1);
                store.SaveChangeset(changeset2);
            }
        }

        [Test]
        public void AggregateTest()
        {
            var userId = Guid.NewGuid().ToString();

            var user = new User(new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            user.Apply(new UserNameChangedEvent()
            {
                UserId = userId,
                FirstName = "John Jr.",
                LastName = "Melinda III"
            });

            user.Apply(new UserEmailChangedEvent()
            {
                UserId = userId,
                Email = "john.jr@test.com",
            });

            var store = Helper.GetEventStore();

            using (new AggregateCleanuper(Helper.GetEventServer(), user.Id))
            {
                store.SaveChangeset(user.CreateChangeset());

                var persisted = AggregateCreator.CreateAggregateRoot<User>();
                var aggregateChangesetStream = store.GetChangesetStream(userId);
                persisted.LoadsFromChangesetStream(aggregateChangesetStream);

                Assert.AreEqual(persisted.Id, userId);
                Assert.AreEqual(persisted.Version, 1);
                Assert.AreEqual(GetPrivateValue<String>(persisted, "_firstName"), "John Jr.");
                Assert.AreEqual(GetPrivateValue<String>(persisted, "_lastName"), "Melinda III");
                Assert.AreEqual(GetPrivateValue<String>(persisted, "_email"), "john.jr@test.com");
            }
        }

        public T InvoikeConstructor<T>()
        {
            var constructor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            var obj = (T)constructor.Invoke(null);
            return obj;
        }


        [Test]
        public void RepositoryTest()
        {
            var userId = Guid.NewGuid().ToString();

            var user = new User(new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            user.Apply(new UserNameChangedEvent()
            {
                UserId = userId,
                FirstName = "John Jr.",
                LastName = "Melinda III"
            });

            user.Apply(new UserEmailChangedEvent()
            {
                UserId = userId,
                Email = "john.jr@test.com",
            });

            var repository = Helper.GetUserRepository();

            using (new AggregateCleanuper(Helper.GetEventServer(), user.Id))
            {
                repository.Save(user);

                var stored = repository.GetById(userId);

                Assert.AreEqual(stored.Id, userId);
                Assert.AreEqual(stored.Version, 1);
                Assert.AreEqual(GetPrivateValue<String>(stored, "_firstName"), "John Jr.");
                Assert.AreEqual(GetPrivateValue<String>(stored, "_lastName"), "Melinda III");
                Assert.AreEqual(GetPrivateValue<String>(stored, "_email"), "john.jr@test.com");
            }
        }

        [Test]
        public void SpeedTest()
        {
            var total = 15;
            var userId = Guid.NewGuid().ToString();

            var user = new User(new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            var repository = Helper.GetUserRepository();

            using (new AggregateCleanuper(Helper.GetEventServer(), user.Id))
            {
                repository.Save(user);

                User stored = null;
                for (int i = 0; i < total; i++)
                {
                    stored = repository.GetById(userId);

                    stored.Apply(new UserNameChangedEvent()
                    {
                        UserId = userId,
                        FirstName = "John Jr. " + i,
                        LastName = "Melinda III " + i
                    });

                    stored.Apply(new UserEmailChangedEvent()
                    {
                        UserId = userId,
                        Email = "john.jr@test.com " + i,
                    });

                    repository.Save(stored);
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                stored = repository.GetById(userId);
                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;

                Assert.AreEqual(stored.Id, userId);
                Assert.AreEqual(stored.Version, total + 1);
                Assert.AreEqual(GetPrivateValue<String>(stored, "_firstName"), "John Jr. " + (total - 1));
                Assert.AreEqual(GetPrivateValue<String>(stored, "_lastName"), "Melinda III " + (total - 1));
                Assert.AreEqual(GetPrivateValue<String>(stored, "_email"), "john.jr@test.com " + (total - 1));
            }
        }

        [Test]
        public void ApplyingEventWithoutHandlerTest()
        {
            var userId = Guid.NewGuid().ToString();

            var user = new User(new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            user.Apply(new UserClickedOnButtonEvent(){
                UserId = userId,
                ButtonText = "Purchase it now!"
            });

            user.Apply(new UserClickedOnButtonEvent()
            {
                UserId = userId,
                ButtonText = "Purchase it tomorrow!"
            });

            var changeset = user.CreateChangeset();

            Assert.AreEqual(changeset.Count, 3);
            Assert.AreEqual(changeset.Events[1].GetType() == typeof(UserClickedOnButtonEvent), true);
            Assert.AreEqual(changeset.Events[2].GetType() == typeof(UserClickedOnButtonEvent), true);

            var repository = Helper.GetUserRepository();
            using (new AggregateCleanuper(Helper.GetEventServer(), user.Id))
            {
                repository.Save(user);

                var stored = repository.GetById(user.Id);

                Assert.AreEqual(stored.Id, userId);
                Assert.AreEqual(stored.Version, 1);
                Assert.AreEqual(GetPrivateValue<String>(stored, "_firstName"), "John");
                Assert.AreEqual(GetPrivateValue<String>(stored, "_lastName"), "Melinda");
                Assert.AreEqual(GetPrivateValue<String>(stored, "_email"), "test@test.com");

            }
        }

        [Test]
        public void VersionOfNewAggregateShouldBeZero()
        {
            var user = new User(new UserCreatedEvent()
            {
                UserId = "3333",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            Assert.AreEqual(user.Version, 0);
        }

        [Test]
        public void VersionOfNewlySavedAggregateShouldBeOne()
        {
            var userId = Guid.NewGuid().ToString();
            var user = new User(new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            Assert.AreEqual(user.Version, 0);

            var repository = Helper.GetUserRepository();
            using (new AggregateCleanuper(Helper.GetEventServer(), user.Id))
            {
                repository.Save(user);
                var stored = repository.GetById(user.Id);

                Assert.AreEqual(stored.Id, userId);
                Assert.AreEqual(stored.Version, 1);
            }
        }

        private T GetPrivateValue<T>(Object obj, String name)
        {
            var privateValue = (T) obj.GetType().InvokeMember(name,
                BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic,
                null, obj, null);
            return privateValue;
        }
    }
}
