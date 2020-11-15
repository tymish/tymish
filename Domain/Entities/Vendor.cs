using System;

namespace Tymish.Domain.Entities
{
    public class Vendor
    {
        public Guid Id { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string Email { get; set; }
        public string? MobilePhone { get; set; }
        public string? Password { get; set; }
        public decimal HourlyPay { get; set; }
        public DateTime Created { get; private set; }
        public DateTime? Registered { get; set; }
        public Vendor(string email)
        {
            Email = email;
            Created = DateTime.Now;
        }
    }
}