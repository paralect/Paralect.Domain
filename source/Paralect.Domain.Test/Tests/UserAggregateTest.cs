using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using Paralect.Domain.Test.Aggregates;
using Paralect.Domain.Test.Cleanupers;
using Paralect.Domain.Test.Events;
using Paralect.Domain.Utilities;

namespace Paralect.Domain.Test.Tests
{
    [TestFixture]
    public class UserAggregateTest
    {
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

            var store = Helper.GetRepository();

            using (new AggregateCleanuper(user.Id))
            {
                store.Save(user);

                var persisted = store.GetById<User>(userId);

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

            var repository = Helper.GetRepository();

            using (new AggregateCleanuper(user.Id))
            {
                repository.Save(user);

                var stored = repository.GetById<User>(userId);

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
            var total = 50;
            var userId = Guid.NewGuid().ToString();

            var user = new User(new UserCreatedEvent()
            {
                UserId = userId,
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            });

            var repository = Helper.GetRepository();

            using (new AggregateCleanuper(user.Id))
            {
                repository.Save(user);

                User stored = null;
                for (int i = 0; i < total; i++)
                {
                    stored = repository.GetById<User>(userId);

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
                stored = repository.GetById<User>(userId);
                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;

                Assert.AreEqual(stored.Id, userId);
                Assert.AreEqual(stored.Version, total + 1);
                Assert.AreEqual(GetPrivateValue<String>(stored, "_firstName"), "John Jr. " + (total - 1));
                Assert.AreEqual(GetPrivateValue<String>(stored, "_lastName"), "Melinda III " + (total - 1));
                Assert.AreEqual(GetPrivateValue<String>(stored, "_email"), "john.jr@test.com " + (total - 1));
            }
        }

        [Ignore]
        public void LoadTest()
        {
            //6445c3ec-b2df-4252-b093-4ca748f1de37

            var repository = Helper.GetRepository();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var user = repository.GetById<User>("6445c3ec-b2df-4252-b093-4ca748f1de37");
            stopwatch.Stop();

            var elapsed = stopwatch.ElapsedMilliseconds;

            

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

            var repository = Helper.GetRepository();
            using (new AggregateCleanuper(user.Id))
            {
                repository.Save(user);

                var stored = repository.GetById<User>(user.Id);

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
        public void PossibleToLoadFromEvents()
        {
            var user = AggregateCreator.CreateAggregateRoot<User>();

            var created = new UserCreatedEvent()
            {
                UserId = "3333",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Melinda"
            };

            var changed = new UserNameChangedEvent()
            {
                UserId = "3333",
                FirstName = "John Jr.",
                LastName = "Melinda III"
            };

            user.LoadFromEvents(new List<IEvent> { created, changed });

            Assert.AreEqual(user.Id, created.UserId);
            Assert.AreEqual(user.Version, 1);
            Assert.AreEqual(GetPrivateValue<String>(user, "_firstName"), changed.FirstName);
            Assert.AreEqual(GetPrivateValue<String>(user, "_lastName"), changed.LastName);
            Assert.AreEqual(GetPrivateValue<String>(user, "_email"), created.Email);
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

            var repository = Helper.GetRepository();
            using (new AggregateCleanuper(user.Id))
            {
                repository.Save(user);
                var stored = repository.GetById<User>(user.Id);

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
