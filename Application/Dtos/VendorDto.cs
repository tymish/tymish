using System;

namespace Tymish.Application.Dtos
{
    public class VendorDto
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public decimal HourlyPay { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Registered { get; set; }
    }   
}