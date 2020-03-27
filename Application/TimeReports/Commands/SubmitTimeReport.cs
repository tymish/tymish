using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Commands
{
    public class SubmitTimeReportCommand : IRequest<TimeReport>
    {
        public Guid TimeReportId { get; set; }
        public IList<TimeEntry> TimeEntries { get; set; }
    }

    public class SubmitTimeReportHandler : IRequestHandler<SubmitTimeReportCommand, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public SubmitTimeReportHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<TimeReport> Handle(SubmitTimeReportCommand request, CancellationToken cancellationToken)
        {
            var timeReport = await _context.Set<TimeReport>()
                .SingleOrDefaultAsync(
                    e => e.Id == request.TimeReportId,
                    cancellationToken
                );
            
            if (timeReport == default(TimeReport))
            {
                throw new NotFoundException(nameof(TimeReport), request.TimeReportId);
            }
            
            timeReport.TimeEntries = request.TimeEntries;
            timeReport.Submitted = DateTime.UtcNow;

            _context.Set<TimeReport>().Update(timeReport);

            await _context.SaveChangesAsync(cancellationToken);

            return timeReport;
        }
    }
}