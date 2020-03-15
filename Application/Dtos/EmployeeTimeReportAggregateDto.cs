using System;
using Tymish.Domain.Entities;

namespace Tymish.Application.Dtos
{
    public class EmployeeTimeReportAggregateDto
    {
        public Guid TimeReportId { get; set; }
        public Employee Employee { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Submitted { get; set; }
        public DateTime Paid { get; set; }
        public decimal AmountOwed { get; set; }
    }
}