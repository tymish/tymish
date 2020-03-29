using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Tymish.Application.Dtos;

namespace Tymish.Application.TimeReports.Query
{
    public class GetMonthlyAggregateQuery : IRequest<MonthlyAggregateDto>
    {
        public DateTime Sent { get; set; }
    }

    public class GetMonthlyTimeReportAggregateHandler
        : IRequestHandler<GetMonthlyAggregateQuery, MonthlyAggregateDto>
    {
        private readonly ITymishDbContext _context;

        public GetMonthlyTimeReportAggregateHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlyAggregateDto> Handle(GetMonthlyAggregateQuery request, CancellationToken cancellationToken)
        {
            var timeReports = await _context.Set<TimeReport>()
                .Include(e => e.Employee)
                .Where(e
                    => e.Sent.HasValue
                    && e.Sent.Value.Month == request.Sent.Month
                    && e.Sent.Value.Year == request.Sent.Year)
                .ToListAsync(cancellationToken);

            var monthAggregate = new MonthlyAggregateDto
            {
                Sent = new DateTime(request.Sent.Year, request.Sent.Month, 1),
                ReportsSentCount = timeReports
                    .Where(e => e.Sent.HasValue)
                    .Count(),
                ReportsSubmittedCount = timeReports
                    .Where(e => e.Submitted.HasValue)
                    .Count(),
                ReportsPaidCount = timeReports
                    .Where(e => e.Paid.HasValue)
                    .Count(),
                AmountOwing = timeReports
                    .Select(e=> this.CalculateAmountOwing(e.Employee, e.TimeEntries))
                    .Sum(),
                AmountPaid = 0  // TODO
            };

            return monthAggregate;
        }

        public decimal CalculateAmountOwing(Employee employee, IList<TimeEntry> timeEntries)
        {
            if (timeEntries == null)
            {
                return 0M;
            }
            return timeEntries
                .Select(e => (e.End - e.Start).Hours * employee.HourlyPay)
                .Sum();
        }
    }
}