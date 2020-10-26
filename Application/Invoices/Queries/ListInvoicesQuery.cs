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
            var invoices = await _context
                .Set<Invoice>()
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