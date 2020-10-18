using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using AutoMapper;
using Tymish.Application.Dtos;

namespace Tymish.Application.Invoices.Query
{
    public class ListInvoicesForVendorQuery : IRequest<IList<InvoiceDto>>
    {
        public Guid VendorId { get; set; }
        public ListInvoicesForVendorQuery(Guid vendorId)
        {
            VendorId = vendorId;
        }
    }

    public class ListInvoicesForVendorHandler : IRequestHandler<ListInvoicesForVendorQuery, IList<InvoiceDto>>
    {
        private readonly ITymishDbContext _context;
        private readonly IMapper _mapper;
        public ListInvoicesForVendorHandler(
            ITymishDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<InvoiceDto>> Handle(ListInvoicesForVendorQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _context
                .Set<Invoice>()
                .Where(invoice
                    => invoice.VendorId == request.VendorId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<Invoice>, IList<InvoiceDto>>(invoices);
        }
    }
}