using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.Invoices.Commands
{
    public class SendInvoiceCommand : IRequest<Invoice>
    {
        public Guid InvoiceId { get; set; }
    }

    public class SendInvoiceHandler : IRequestHandler<SendInvoiceCommand, Invoice>
    {
        private readonly ITymishDbContext _context;
        private readonly IEmailGateway _email;

        public SendInvoiceHandler(ITymishDbContext context, IEmailGateway email) {
            _context = context;
            _email = email;
        }

        public async Task<Invoice> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Set<Invoice>()
                .SingleOrDefaultAsync(e
                    => e.Id == request.InvoiceId,
                    cancellationToken
                );
            
            if (invoice == default(Invoice))
            {
                // TODO: This exception message is hard to debug
                throw new NotFoundException(nameof(Invoice), request.InvoiceId);
            }

            invoice.Sent = DateTime.UtcNow;

            _context.Set<Invoice>().Update(invoice);

            await Task.WhenAll(
                _context.SaveChangesAsync(cancellationToken),
                _email.Send(
                    invoice.Employee.Email,
                    "Submit your invoice",
                    $"https://localhost:4200/submit-invoice/{invoice.Id}")
            );

            return invoice;
        }
    }
}