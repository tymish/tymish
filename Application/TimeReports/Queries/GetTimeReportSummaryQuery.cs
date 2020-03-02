using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportSummaryQuery : IRequest<TimeReportSummary>
    {
        public int IssuedMonth { get; set; }
        public int IssuedYear { get; set; }
    }

    public class TimeReportSummary
    {
        public int IssuedMonth { get; set; }
        public int IssuedYear { get; set; }
        public int ReportsIssuedCount { get; set; }
        public int ReportsSubmittedCount { get; set; }
        public int ReportsPaidCount { get; set; }
        public decimal AmountOwing { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class GetTimeReportSummaryHandler : IRequestHandler<GetTimeReportSummaryQuery, TimeReportSummary>
    {
        private readonly ITymishDbContext _context;

        public GetTimeReportSummaryHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<TimeReportSummary> Handle(GetTimeReportSummaryQuery request, CancellationToken cancellationToken)
        {
            var timeReports = await _context.Set<TimeReport>()
                .Where(e
                    => e.Issued.Month == request.IssuedMonth
                    && e.Issued.Year == request.IssuedYear)
                .ToListAsync();

            var summary = new TimeReportSummary
            {
                IssuedMonth = request.IssuedMonth,
                IssuedYear = request.IssuedYear,
                ReportsIssuedCount = timeReports
                    .Where(e => e.Issued != default(DateTime))
                    .Count(),
                ReportsSubmittedCount = timeReports
                    .Where(e => e.Submitted != default(DateTime))
                    .Count(),
                ReportsPaidCount = timeReports
                    .Where(e => e.Paid != default(DateTime))
                    .Count(),
                AmountOwing = 0, // TODO
                AmountPaid = 0  // TODO
            };

            return summary;
        }
    }
}