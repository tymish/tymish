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
    public class GetVendorInvoiceQuery : IRequest<Invoice>
    {   
        public Guid InvoiceId { get; set; }
    }

    public class GetVendorInvoiceHandler : IRequestHandler<GetVendorInvoiceQuery, Invoice>
    {
        private readonly ITymishDbContext _context;
        public GetVendorInvoiceHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<Invoice> Handle(GetVendorInvoiceQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Invoice>()
                .SingleOrDefaultAsync(e => e.Id == request.InvoiceId);

            if (entity == default(Invoice))
            {
                throw new NotFoundException(nameof(Invoice), request.InvoiceId);
            }

            return entity;
        }
    }
}