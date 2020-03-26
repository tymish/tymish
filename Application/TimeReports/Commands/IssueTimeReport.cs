using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Commands
{
    public class SendTimeReportCommand : IRequest<TimeReport>
    {
        public int EmployeeNumber { get; set; }
    }

    public class SendTimeReportHandler : IRequestHandler<SendTimeReportCommand, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public SendTimeReportHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<TimeReport> Handle(SendTimeReportCommand request, CancellationToken cancellationToken)
        {
            var timeReport = await _context.Set<TimeReport>()
                .SingleOrDefaultAsync(e
                    => e.Employee.EmployeeNumber == request.EmployeeNumber
                    && e.Sent == default(DateTime),
                    cancellationToken
                );
            
            if (timeReport == default(TimeReport))
            {
                // TODO: This exception message is bad
                throw new NotFoundException(nameof(TimeReport), request.EmployeeNumber);
            }

            timeReport.Sent = DateTime.UtcNow;

            _context.Set<TimeReport>().Update(timeReport);

            await _context.SaveChangesAsync(cancellationToken);

            return timeReport;
        }
    }
}