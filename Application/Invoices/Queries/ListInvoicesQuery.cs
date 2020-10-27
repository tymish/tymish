using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;
using Tymish.Application.Dtos;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Tymish.Application.Invoices.Query
{
    public class ListInvoicesQuery : IRequest<IList<InvoiceDto>>
    {
        public InvoiceStatus Status { get; set; }
    }

    public class ListInvoicesHandler : IRequestHandler<ListInvoicesQuery, IList<InvoiceDto>>
    {
        private readonly ITymishDbContext _context;
        private readonly IMapper _mapper;
        public ListInvoicesHandler(
            ITymishDbContext context,
            IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<InvoiceDto>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Invoice, bool>> statusFilter = request.Status switch
            {
                InvoiceStatus.Submitted => invoice => invoice.Submitted != null,
                InvoiceStatus.Paid => invoice => invoice.Paid != null,
                _ => invoice => true
            };

            var invoices = await _context
                .Set<Invoice>()
                .Include(e => e.Vendor)
                .Where(statusFilter)
                .ToListAsync(cancellationToken);
            
            return _mapper.Map<List<Invoice>, IList<InvoiceDto>>(invoices);
        }
    }

    public enum InvoiceStatus
    {
        Created,
        Submitted,
        Approved,
        Paid
    }
}