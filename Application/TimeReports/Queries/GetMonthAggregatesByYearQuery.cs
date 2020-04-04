using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Dtos;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.TimeReports.Query
{
    public class GetMonthAggregateByYearQuery : IRequest<IList<MonthAggregateDto>>
    {
        public int Year { get; set; }
    }

    public class GetMonthAggregateByYearHandler
        : IRequestHandler<GetMonthAggregateByYearQuery, IList<MonthAggregateDto>>
    {
        ITymishDbContext _context;

        public GetMonthAggregateByYearHandler(ITymishDbContext context)
        {
            _context = context;
        }
        public async Task<IList<MonthAggregateDto>> Handle(GetMonthAggregateByYearQuery request, CancellationToken cancellationToken)
        {
            var timeReports = await _context.Set<TimeReport>()
                .Where(e => e.PayPeriod.Year == request.Year)
                .Include(e => e.Employee)
                .ToListAsync(cancellationToken);

            var aggregateDtos = timeReports
                .GroupBy(e => e.PayPeriod)
                .Select(e => new MonthAggregateDto
                {
                    PayPeriod = e.Key,
                    SentReports = e.Where(report => report.Sent.HasValue).Count(), 
                    ReceivedReports = e.Where(report => report.Submitted.HasValue).Count(),
                    PaidReports = e.Where(report => report.Paid.HasValue).Count(),
                    TotalOwing = SumAmountOwing(e.ToList())
                })
                .OrderByDescending(x => x.PayPeriod)
                .ToList();
            
            return aggregateDtos;
        }

        public decimal SumAmountOwing(IList<TimeReport> reports)
        {
            if (reports == null) return 0M;
            return reports.Sum(e => SumTimeEntryHours(e.TimeEntries) * e.Employee.HourlyPay);
        }

        public decimal SumTimeEntryHours(IList<TimeEntry> entries)
        {
            if (entries == null) return 0M;
            return entries.Sum(e => (e.End - e.Start).Hours);
        }
    }
}