using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Tymish.Application.Dtos;

namespace Tymish.Application.TimeReports.Query
{
    public class GetMonthlyAggregateQuery : IRequest<MonthlyAggregateDto>
    {
        public DateTime IssuedMonth { get; set; }
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
                    => e.Issued.Month == request.IssuedMonth.Month
                    && e.Issued.Year == request.IssuedMonth.Year)
                .ToListAsync(cancellationToken);

            var monthAggregate = new MonthlyAggregateDto
            {
                Issued = new DateTime(request.IssuedMonth.Year, request.IssuedMonth.Month, 1),
                ReportsIssuedCount = timeReports
                    .Where(e => e.Issued != default(DateTime))
                    .Count(),
                ReportsSubmittedCount = timeReports
                    .Where(e => e.Submitted != default(DateTime))
                    .Count(),
                ReportsPaidCount = timeReports
                    .Where(e => e.Paid != default(DateTime))
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