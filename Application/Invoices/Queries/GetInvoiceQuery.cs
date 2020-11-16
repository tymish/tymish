using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using Tymish.Application.Dtos;
using AutoMapper;

namespace Tymish.Application.Invoices.Query
{
    public class GetInvoiceQuery : IRequest<InvoiceDto>
    {
        public Guid InvoiceId { get; set; }
        public GetInvoiceQuery(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }

    public class GetInvoiceHandler : IRequestHandler<GetInvoiceQuery, InvoiceDto>
    {
        private readonly ITymishDbContext _context;
        private readonly IMapper _mapper;
        public GetInvoiceHandler(
            ITymishDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context
                .Set<Invoice>()
                .Include(e => e.Vendor)
                .SingleOrDefaultAsync(invoice
                    => invoice.Id == request.InvoiceId);

            if (invoice == default(Invoice))
            {
                throw new NotFoundException(nameof(Invoice), request.InvoiceId);
            }

            return _mapper.Map<Invoice, InvoiceDto>(invoice);
        }
    }
}