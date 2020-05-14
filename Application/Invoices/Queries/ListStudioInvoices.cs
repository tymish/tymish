using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace Tymish.Application.Invoices.Query
{
    public class ListStudioInvoicesQuery : IRequest<IList<Invoice>>
    {
        public Guid StudioId { get; set; }
    }

    public class ListStudioInvoicesHandler : IRequestHandler<ListStudioInvoicesQuery, IList<Invoice>>
    {
        private readonly ITymishDbContext _context;
        public ListStudioInvoicesHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<Invoice>> Handle(ListStudioInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = _context.Set<Studio>()
                .Single(e => e.Id == request.StudioId)
                .Invoices
                .ToList();

            return invoices;
        }
    }
}