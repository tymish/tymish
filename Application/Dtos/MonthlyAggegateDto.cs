using System;

namespace Tymish.Application.Dtos
{
    public class MonthAggregateDto
    {
        public DateTime PayPeriod { get; set; }
        public int SentReports { get; set; }
        public int ReceivedReports { get; set; }
        public int PaidReports { get; set; }
        public decimal TotalOwing { get; set; }

        public MonthAggregateDto() {}
        public MonthAggregateDto(DateTime payPeriod)
        {
            PayPeriod = payPeriod;
            SentReports = 0;
            ReceivedReports = 0;
            PaidReports = 0;
            TotalOwing = 0m;
        }
    }
}