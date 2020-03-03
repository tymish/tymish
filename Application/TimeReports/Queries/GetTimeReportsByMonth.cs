using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportsByMonthQuery : IRequest<IList<Employee>>
    {
        public int IssuedMonth { get; set; }
        public int IssuedYear { get; set; }
    }

    public class GetTimeReportsByMonthHandler : IRequestHandler<GetTimeReportsByMonthQuery, IList<Employee>>
    {
        private readonly ITymishDbContext _context;

        public GetTimeReportsByMonthHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<Employee>> Handle(GetTimeReportsByMonthQuery request, CancellationToken cancellationToken)
        {
            var employees = await _context.Set<Employee>()
                .Include(e => e.TimeReports)
                .Where(e => e.TimeReports.Any(report
                    => report.Issued.Month == request.IssuedMonth
                    && report.Issued.Year == request.IssuedYear))
                .ToListAsync(cancellationToken);

            foreach (var employee in employees)
            {
                employee.TimeReports = employee.TimeReports
                    .Where(e
                        => e.Issued.Month == request.IssuedMonth
                        && e.Issued.Year == request.IssuedYear)
                    .ToList();
            }

            return employees;
        }
    }
}