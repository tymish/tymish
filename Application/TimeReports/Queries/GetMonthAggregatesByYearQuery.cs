using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Dtos;
using Tymish.Application.Interfaces;
using Tymish.Domain.Entities;

namespace Tymish.Application.Invoices.Query
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
            var invoices = await _context.Set<Invoice>()
                .Where(e => e.PayPeriod.Year == request.Year)
                .Include(e => e.Employee)
                .ToListAsync(cancellationToken);

            var aggregateDtos = invoices
                .GroupBy(e => e.PayPeriod)
                .Select(e => new MonthAggregateDto
                {
                    PayPeriod = e.Key,
                    SentInvoices = e.Where(invoice => invoice.Sent.HasValue).Count(), 
                    ReceivedInvoices = e.Where(invoice => invoice.Submitted.HasValue).Count(),
                    PaidInvoices = e.Where(invoice => invoice.Paid.HasValue).Count(),
                    TotalOwing = SumAmountOwing(e.ToList())
                })
                .OrderByDescending(e => e.PayPeriod)
                .ToList();
            
            // Get months from January to Now
            var currentYearPastMonths = new List<MonthAggregateDto>();
            for (var month = 1; month <= DateTime.Today.Month; month++) 
            {
                var payPeriod = new DateTime(DateTime.Today.Year, month, 1);
                
                var aggregateDto = aggregateDtos.Exists(e => e.PayPeriod == payPeriod)
                    ? aggregateDtos.Single(e => e.PayPeriod == payPeriod)
                    : new MonthAggregateDto(payPeriod);
                
                currentYearPastMonths.Add(aggregateDto);
            }
            
            return DateTime.Today.Year == request.Year
                ? currentYearPastMonths.OrderByDescending(e => e.PayPeriod).ToList()
                : aggregateDtos;
        }

        public decimal SumAmountOwing(IList<Invoice> invoices)
        {
            if (invoices == null) return 0M;
            return invoices.Sum(e => SumTimeEntryHours(e.TimeEntries) * e.Employee.HourlyPay);
        }

        public decimal SumTimeEntryHours(IList<TimeEntry> entries)
        {
            if (entries == null) return 0M;
            return entries.Sum(e => (e.End - e.Start).Hours);
        }
    }
}