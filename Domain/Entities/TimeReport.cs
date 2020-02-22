using System;
using System.Collections.Generic;

namespace Tymish.Domain.Entities
{
    public class TimeReport
    {
        public Guid Id { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Submitted { get; set; }
        public DateTime Approved { get; set; }
        public IList<TimeEntry> TimeEntries { get; set; }
    }
}