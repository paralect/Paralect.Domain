using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domain.Test.Events
{
    public class UserCreatedEvent : Event
    {
        public String UserId { get; set; }
        public Int32 Year { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }

        public String Email { get; set; }
    }
}
