using System;

namespace Core.Entities
{
    public class Employee
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public Decimal HourlyPay { get; set; }
    }
}