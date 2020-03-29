using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Commands
{
    public class SendTimeReportCommand : IRequest<TimeReport>
    {
        public Guid TimeReportId { get; set; }
    }

    public class SendTimeReportHandler : IRequestHandler<SendTimeReportCommand, TimeReport>
    {
        private readonly ITymishDbContext _context;
        private readonly IEmailGateway _email;

        public SendTimeReportHandler(ITymishDbContext context, IEmailGateway email) {
            _context = context;
            _email = email;
        }

        public async Task<TimeReport> Handle(SendTimeReportCommand request, CancellationToken cancellationToken)
        {
            var timeReport = await _context.Set<TimeReport>()
                .SingleOrDefaultAsync(e
                    => e.Id == request.TimeReportId,
                    cancellationToken
                );
            
            if (timeReport == default(TimeReport))
            {
                // TODO: This exception message is hard to debug
                throw new NotFoundException(nameof(TimeReport), request.TimeReportId);
            }

            timeReport.Sent = DateTime.UtcNow;

            _context.Set<TimeReport>().Update(timeReport);

            await Task.WhenAll(
                _context.SaveChangesAsync(cancellationToken),
                _email.Send(
                    timeReport.Employee.Email,
                    "Submit your time report",
                    $"https://localhost:4200/submit-time-report/{timeReport.Id}")
            );

            return timeReport;
        }
    }
}