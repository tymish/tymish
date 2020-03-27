using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.Linq;

namespace Tymish.Application.TimeReports.Commands
{
    /// <summary>Sends time reports to employees.
    /// If no time report exists, it creates one.</summary>
    public class SendTimeReportsCommand : IRequest
    {
        public DateTime Sent { get; set; }
    }

    public class SendTimeReportsHandler : IRequestHandler<SendTimeReportsCommand, Unit>
    {
        private readonly ITymishDbContext _context;
        private readonly IMediator _mediator;

        public SendTimeReportsHandler(ITymishDbContext context, IMediator mediator) {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(SendTimeReportsCommand request, CancellationToken cancellationToken)
        {
            var employees = await _context.Set<Employee>()
                .Include(x => x.TimeReports)
                .ToListAsync(cancellationToken);

            var emailList = new List<(string, Guid)>();

            foreach(var employee in employees)
            {
                // Check if they have a time report for this month
                var alreadyHasTimeReportForThisMonth = employee.TimeReports.Any(x
                    => x.Sent.Month == request.Sent.Month
                    && x.Sent.Year == request.Sent.Year
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
                    Sent = DateTime.UtcNow
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