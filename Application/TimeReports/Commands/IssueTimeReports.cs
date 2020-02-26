using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.Linq;

namespace Tymish.Application.TimeReports.Commands
{
    public class IssueTimeReportsCommand : IRequest
    {
        public IList<int> EmployeeNumbers { get; set; }
    }

    public class IssueTimeReportsHandler : IRequestHandler<IssueTimeReportsCommand, Unit>
    {
        private readonly ITymishDbContext _context;
        private readonly IMediator _mediator;

        public IssueTimeReportsHandler(ITymishDbContext context, IMediator mediator) {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(IssueTimeReportsCommand request, CancellationToken cancellationToken)
        {
            // Get time reports for an employeeNumber
            var timeReports = await _context.Set<TimeReport>()
                .Where(e => request.EmployeeNumbers
                    .Any(num => e.Employee.EmployeeNumber == num)
                )
                .ToListAsync(cancellationToken);

            // Set Issued Date
            timeReports = timeReports.Select(e => {
                e.Issued = DateTime.UtcNow;
                return e;
            }).ToList();

            _context.Set<TimeReport>().UpdateRange(timeReports);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}