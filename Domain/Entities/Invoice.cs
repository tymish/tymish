using System;
using System.Collections.Generic;

namespace Tymish.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public DateTime PayPeriod { get; set; }
        public DateTime? Sent { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Paid { get; set; }
        public IList<TimeEntry>? TimeEntries { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }

    public class TimeEntry
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }

        public TimeEntry()
        {
            Comments = string.Empty;
        }
    }
}