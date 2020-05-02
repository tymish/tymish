using System;

namespace Tymish.Application.Dtos
{
    public class MonthAggregateDto
    {
        public DateTime PayPeriod { get; set; }
        public int SentInvoices { get; set; }
        public int ReceivedInvoices { get; set; }
        public int PaidInvoices { get; set; }
        public decimal TotalOwing { get; set; }

        public MonthAggregateDto() {}
        public MonthAggregateDto(DateTime payPeriod)
        {
            PayPeriod = payPeriod;
            SentInvoices = 0;
            ReceivedInvoices = 0;
            PaidInvoices = 0;
            TotalOwing = 0m;
        }
    }
}