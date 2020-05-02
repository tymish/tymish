using System;
using Tymish.Domain.Entities;

namespace Tymish.Application.Dtos
{
    public class EmployeeInvoiceAggregateDto
    {
        public Guid InvoiceId { get; set; }
        public Employee Employee { get; set; }
        public DateTime? Sent { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Paid { get; set; }
        public decimal AmountOwed { get; set; }
    }
}