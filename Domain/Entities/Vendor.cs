using System;
using System.Collections.Generic;

namespace Tymish.Domain.Entities
{
    public class Vendor
    {
        public int Number { get; set; }
        public decimal HourlyRate { get; set; }
        public virtual ICollection<Studio>? Studios { get; set; }
        public virtual IList<Invoice> Invoices { get; set; }

        public Vendor()
        {
            this.Invoices = new List<Invoice>();
        }
    }
}