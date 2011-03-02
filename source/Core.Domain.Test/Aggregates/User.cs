using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Domain.Test.Events;

namespace Core.Domain.Test.Aggregates
{
    public class User : AggregateRoot
    {
        private String _firstName;
        private String _lastName;
        private String _email;

        /// <summary>
        /// Close access to default constructor for aggregate consumers
        /// </summary>
        private User() { }

        public User(UserCreatedEvent createdEvent)
        {
            Apply(createdEvent);
        }

        private void On(UserCreatedEvent created)
        {
            _id = created.UserId;
            _firstName = created.FirstName;
            _lastName = created.LastName;
            _email = created.Email;
        }

        private void On(UserNameChangedEvent created)
        {
            _firstName = created.FirstName;
            _lastName = created.LastName;
        }

        private void On(UserEmailChangedEvent changed)
        {
            _email = changed.Email;
        }

        

    }
}
