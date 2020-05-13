using System;
using System.Collections.Generic;

namespace Tymish.Domain.Entities
{
    public class Studio
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }

        /// <summary>VendorStudios entity manage many-to-many relationship with Auth0 Vendor</summary>
        public virtual ICollection<VendorStudio>? VendorsStudios { get; set; }
    }
}