using System;

namespace Tymish.Domain.Entities
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string Email { get; set; }
        public decimal HourlyPay { get; set; }
        public Vendor(string email)
        {
            Email = email;
        }
    }
}