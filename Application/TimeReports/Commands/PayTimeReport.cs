using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Tymish.Application.TimeReports.Commands
{
    public class PayTimeReportCommand : IRequest<TimeReport>
    {
        /// <summary>TimeReport.Id</summary>
        public Guid Id { get; set; }
        [Required]
        public string reference { get; set; }
    }

    public class PayTimeReportHandler : IRequestHandler<PayTimeReportCommand, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public PayTimeReportHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<TimeReport> Handle(PayTimeReportCommand request, CancellationToken cancellationToken)
        {
            var timeReport = await _context.Set<TimeReport>()
                .SingleOrDefaultAsync(
                    e => e.Id == request.Id,
                    cancellationToken
                );
            
            if (timeReport == default(TimeReport))
            {
                throw new NotFoundException(nameof(TimeReport), request.Id);
            }
            
            timeReport.Paid = DateTime.UtcNow;

            _context.Set<TimeReport>().Update(timeReport);

            await _context.SaveChangesAsync(cancellationToken);

            return timeReport;
        }
    }
}