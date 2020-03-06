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
        public DateTime Issued { get; set; }
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
            var employees = await _context.Set<Employee>()
                .Include(x => x.TimeReports)
                .ToListAsync(cancellationToken);

            var emailList = new List<(string, Guid)>();

            foreach(var employee in employees)
            {
                // Check if they have a time report for this month
                var alreadyHasTimeReportForThisMonth = employee.TimeReports.Any(x
                    => x.Issued.Month == request.Issued.Month
                    && x.Issued.Year == request.Issued.Year
                );

                if (alreadyHasTimeReportForThisMonth)
                {
                    continue; // Skip this employee
                }
                
                var timeReportId = new Guid();

                emailList.Add((employee.Email, timeReportId));

                employee.TimeReports.Add(new TimeReport()
                {
                    Id = timeReportId,
                    EmployeeId = employee.Id,
                    Issued = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Send email to the employees
            foreach(var email in emailList)
            {
                var address = email.Item1;
                var timeSheetUrl = ""; // makeUrl(email.Item2);
                // Send email with link to their most recent 
                // smptGateway.Send(address, timeSheetUrl);
            }

            return Unit.Value;
        }
    }
}