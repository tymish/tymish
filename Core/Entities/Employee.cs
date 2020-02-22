using System;

namespace Core.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public Decimal HourlyPay { get; set; }
    }
}