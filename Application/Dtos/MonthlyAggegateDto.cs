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
    }
}