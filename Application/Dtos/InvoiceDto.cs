using System;
using System.Collections.Generic;
using System.Linq;

namespace Tymish.Application.Dtos
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Submitted { get; set; }
        public List<TimeEntryDto> TimeEntryDtos { get; set; }
        public double TotalHours
        {
            get => TimeEntryDtos.Sum(x => x.TotalHours);
        }
        public decimal TotalAmount { get; set; }
        public DateTime? Paid { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentReference { get; set; }
        public Guid VendorId { get; set; }
        public InvoiceDto()
        {
            TimeEntryDtos = new List<TimeEntryDto>();
        }
    }

    public class TimeEntryDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
        public double TotalHours { get => (End - Start).TotalHours; }
    }
}