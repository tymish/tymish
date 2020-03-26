using System;

namespace Tymish.Application.Dtos
{
    public class MonthlyAggregateDto
    {
        public DateTime Sent { get; set; }
        public int ReportsSentCount { get; set; }
        public int ReportsSubmittedCount { get; set; }
        public int ReportsPaidCount { get; set; }
        public decimal AmountOwing { get; set; }
        public decimal AmountPaid { get; set; }
    }
}