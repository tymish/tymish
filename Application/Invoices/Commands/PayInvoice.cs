using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Tymish.Application.Invoices.Commands
{
    public class PayInvoiceCommand : IRequest<Invoice>
    {
        public Guid InvoiceId { get; set; }
        [Required]
        [Range(0,10000)]
        public Decimal PaymentAmount { get; set; }
        [Required]
        public string PaymentReference { get; set; }
    }

    public class PayInvoiceHandler : IRequestHandler<PayInvoiceCommand, Invoice>
    {
        private readonly ITymishDbContext _context;
        public PayInvoiceHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<Invoice> Handle(PayInvoiceCommand request, CancellationToken cancellationToken)
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
            
            invoice.Paid = DateTime.UtcNow;
            invoice.PaymentAmount = request.PaymentAmount;
            invoice.PaymentReference = request.PaymentReference;

            _context.Set<Invoice>().Update(invoice);

            await _context.SaveChangesAsync(cancellationToken);

            return invoice;
        }
    }
}