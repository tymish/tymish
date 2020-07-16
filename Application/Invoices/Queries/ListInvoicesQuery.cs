using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Tymish.Application.Invoices.Query
{
    public class ListInvoicesQuery : IRequest<IList<Invoice>> {}

    public class ListInvoicesHandler : IRequestHandler<ListInvoicesQuery, IList<Invoice>>
    {
        private readonly ITymishDbContext _context;
        public ListInvoicesHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<Invoice>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await _context
                .Set<Invoice>()
                .ToListAsync(cancellationToken);
        }
    }
}