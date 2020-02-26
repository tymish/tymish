using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace Tymish.Application.TimeReports.Query
{
    public class GetTimeReportsByEmployeeNumberQuery : IRequest<IList<TimeReport>>
    {
        public int EmployeeNumber { get; set; }
    }

    public class GetTimeReportsByEmployeeNumberHandler : IRequestHandler<GetTimeReportsByEmployeeNumberQuery, IList<TimeReport>>
    {
        private readonly ITymishDbContext _context;

        public GetTimeReportsByEmployeeNumberHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<TimeReport>> Handle(GetTimeReportsByEmployeeNumberQuery request, CancellationToken cancellationToken)
        {
            var timeReports = await _context.Set<TimeReport>()
                .Where(e => e.Employee.EmployeeNumber == request.EmployeeNumber)
                .ToListAsync(cancellationToken);

            if (timeReports?.Count == 0)
            {
                // TODO: This exception message is bad
                throw new NotFoundException(nameof(TimeReport), request.EmployeeNumber);
            }

            return timeReports;
        }
    }
}