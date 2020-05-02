using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.Invoices.Commands
{
    public class SubmitInvoiceCommand : IRequest<Invoice>
    {
        public Guid InvoiceId { get; set; }
        public IList<TimeEntry> TimeEntries { get; set; }
    }

    public class SubmitInvoiceHandler : IRequestHandler<SubmitInvoiceCommand, Invoice>
    {
        private readonly ITymishDbContext _context;

        public SubmitInvoiceHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<Invoice> Handle(SubmitInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Set<Invoice>()
                .SingleOrDefaultAsync(
                    e => e.Id == request.InvoiceId,
                    cancellationToken
                );
            
            if (invoice == default(Invoice))
            {
                throw new NotFoundException(nameof(Invoice), request.InvoiceId);
            }
            
            invoice.TimeEntries = request.TimeEntries;
            invoice.Submitted = DateTime.UtcNow;

            _context.Set<Invoice>().Update(invoice);

            await _context.SaveChangesAsync(cancellationToken);

            return invoice;
        }
    }
}