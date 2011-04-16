using System;

namespace Paralect.Transitions.Mongo.Test.Events
{
    public class StreamCreatedEvent
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public Int32 Number { get; set; }
    }
}