using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tymish.Application.Invoices.Query
{
    public class ListInvoicesForVendorQuery : IRequest<IList<Invoice>>
    {
        public Guid VendorId { get; set; }
        public ListInvoicesForVendorQuery(Guid vendorId)
        {
            VendorId = vendorId;
        }
    }

    public class ListInvoicesForVendorHandler : IRequestHandler<ListInvoicesForVendorQuery, IList<Invoice>>
    {
        private readonly ITymishDbContext _context;
        public ListInvoicesForVendorHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<Invoice>> Handle(ListInvoicesForVendorQuery request, CancellationToken cancellationToken)
        {
            return await _context
                .Set<Invoice>()
                .Where(invoice
                    => invoice.VendorId == request.VendorId)
                .ToListAsync(cancellationToken);
        }
    }
}