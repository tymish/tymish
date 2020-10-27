using System;
using System.Collections.Generic;

namespace Tymish.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Submitted { get; set; }
        public IList<TimeEntry>? TimeEntries { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? Paid { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentReference { get; set; }
        public Guid VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }
        public Invoice()
        {
            PaymentReference = "";
        }
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