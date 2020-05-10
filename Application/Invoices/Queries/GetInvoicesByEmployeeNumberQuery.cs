using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace Tymish.Application.Invoices.Query
{
    public class GetInvoicesByEmployeeNumberQuery : IRequest<IList<Invoice>>
    {
        public int EmployeeNumber { get; set; }
    }

    public class GetInvoicesByEmployeeNumberHandler : IRequestHandler<GetInvoicesByEmployeeNumberQuery, IList<Invoice>>
    {
        private readonly ITymishDbContext _context;

        public GetInvoicesByEmployeeNumberHandler(ITymishDbContext context) {
            _context = context;
        }

        public async Task<IList<Invoice>> Handle(GetInvoicesByEmployeeNumberQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _context.Set<Invoice>()
                .Where(e => e.Employee.EmployeeNumber == request.EmployeeNumber)
                .ToListAsync(cancellationToken);

            if (invoices?.Count == 0)
            {
                // TODO: This exception message is bad
                throw new NotFoundException(nameof(Invoice), request.EmployeeNumber);
            }

            return invoices;
        }
    }
}