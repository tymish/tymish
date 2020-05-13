using System;

namespace Tymish.Domain.Entities
{
    public class VendorInvoice
    {
        public string VendorId { get; set; }
        public virtual Guid InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}