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
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Query
{
    public class GetEmployeeTimeReportAggregatesQuery : IRequest<IList<EmployeeTimeReportAggregateDto>>
    {
        public DateTime Sent { get; set; }
    }

    public class GetEmployeeTimeReportAggregatesHandler
        : IRequestHandler<GetEmployeeTimeReportAggregatesQuery, IList<EmployeeTimeReportAggregateDto>>
    {
        private readonly ITymishDbContext _context;

        public GetEmployeeTimeReportAggregatesHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<IList<EmployeeTimeReportAggregateDto>> Handle(
            GetEmployeeTimeReportAggregatesQuery request,
            CancellationToken cancellationToken)
        {
            var aggregates = new List<EmployeeTimeReportAggregateDto>();
            var employees = await _context.Set<Employee>().ToListAsync(cancellationToken);

            foreach (var employee in employees)
            {
                var timeReport = await _context
                    .Set<TimeReport>()
                    .Where(e
                        => e.EmployeeId == employee.Id
                        && e.Sent.Month == request.Sent.Month
                        && e.Sent.Year == request.Sent.Year)
                    .SingleOrDefaultAsync(cancellationToken);

                if (timeReport == default(TimeReport))
                {
                    // TODO: make this exception better
                    continue;
                }

                var aggregate = new EmployeeTimeReportAggregateDto
                {
                    TimeReportId = timeReport.Id,
                    Employee = employee,
                    Sent = timeReport.Sent,
                    Submitted = timeReport.Submitted,
                    Paid = timeReport.Paid
                };

                if (timeReport.TimeEntries == null)
                {
                    aggregate.AmountOwed = 0m;
                }
                else
                {
                    aggregate.AmountOwed = timeReport.TimeEntries
                        .Select(x => (x.End - x.Start).Hours)
                        .Sum() * employee.HourlyPay;
                }
                aggregates.Add(aggregate);
            }

            return aggregates;
        }
    }
}