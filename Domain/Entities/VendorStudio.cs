using System;

namespace Tymish.Domain.Entities
{
    public class VendorStudio
    {
        public string VendorId { get; set; }
        public virtual Guid StudioId { get; set; }
        public virtual Studio Studio { get; set; }
        public decimal HourlyPay { get; set; }
    }
}