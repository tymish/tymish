using System;

namespace Core.Entities
{
    public class TimeEntry
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
    }
}