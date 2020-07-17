using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.Invoices.Query
{
    public class GetInvoiceQuery : IRequest<Invoice>
    {
        public Guid InvoiceId { get; set; }
        public GetInvoiceQuery(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }

    public class GetInvoiceHandler : IRequestHandler<GetInvoiceQuery, Invoice>
    {
        private readonly ITymishDbContext _context;
        public GetInvoiceHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<Invoice> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context
                .Set<Invoice>()
                .SingleOrDefaultAsync(invoice
                    => invoice.Id == request.InvoiceId);

            if (invoice == default(Invoice))
            {
                throw new NotFoundException(nameof(Invoice), request.InvoiceId);
            }

            return invoice;
        }
    }
}