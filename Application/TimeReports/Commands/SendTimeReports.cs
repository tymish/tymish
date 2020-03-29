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
        private readonly IEmailGateway _email;

        public SendTimeReportsHandler(
            ITymishDbContext context,
            IMediator mediator,
            IEmailGateway email)
        {
            _context = context;
            _mediator = mediator;
            _email = email;
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
                var alreadyHasTimeReportForThisMonth = employee.TimeReports
                    .Any(x
                    => x.Sent.HasValue
                    && x.Sent.Value.Month == request.Sent.Month
                    && x.Sent.Value.Year == request.Sent.Year
                );

                if (alreadyHasTimeReportForThisMonth)
                {
                    continue; // Skip this employee
                }
                
                var timeReportId = Guid.NewGuid();

                emailList.Add((employee.Email, timeReportId));

                await _context.Set<TimeReport>().AddAsync(new TimeReport()
                {
                    Id = timeReportId,
                    EmployeeId = employee.Id,
                    Sent = DateTime.UtcNow
                }, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Send email to the employees
            foreach(var email in emailList)
            {
                var address = email.Item1;
                var timeReportUrl = $"https://localhost:4200/submit-time-report/{email.Item2}";
                await _email.Send(
                    email.Item1,
                    "Submit your time report for {month}",
                    timeReportUrl);
            }

            return Unit.Value;
        }
    }
}