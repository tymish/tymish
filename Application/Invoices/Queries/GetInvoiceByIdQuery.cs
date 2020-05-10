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
    public class GetInvoiceByIdQuery : IRequest<Invoice>
    {
        public GetInvoiceByIdQuery(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; set; }
    }

    public class GetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdQuery, Invoice>
    {
        private readonly ITymishDbContext _context;

        public GetInvoiceByIdHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<Invoice> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Invoice>()
                .SingleOrDefaultAsync(e => e.Id == request.Id);

            if (entity == default(Invoice))
            {
                throw new NotFoundException(nameof(Invoice), request.Id);
            }

            return entity;
        }
    }
}