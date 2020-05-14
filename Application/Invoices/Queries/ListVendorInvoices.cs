using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Tymish.Application.Invoices.Query
{
    public class ListVendorInvoicesQuery : IRequest<IList<Invoice>>
    {
        public string VendorId { get; set; }
    }

    public class ListVendorInvoicesHandler : IRequestHandler<ListVendorInvoicesQuery, IList<Invoice>>
    {
        private readonly ITymishDbContext _context;
        public ListVendorInvoicesHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<Invoice>> Handle(ListVendorInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _context.Set<VendorInvoice>()
                .Where(e => e.VendorId == request.VendorId)
                .Select(e => e.Invoice)
                .ToListAsync(cancellationToken);

            return invoices;
        }
    }
}