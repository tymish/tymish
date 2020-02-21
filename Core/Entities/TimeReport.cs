using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class TimeReport
    {
        public DateTime Issued { get; set; }
        public DateTime Submitted { get; set; }
        public DateTime Approved { get; set; }
        public IList<TimeEntry> TimeEntries { get; set; }
    }
}