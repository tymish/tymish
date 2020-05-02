using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using Tymish.Domain.Entities;
using Tymish.Application.Interfaces;

namespace Tymish.Application.Employees.Queries
{
    public class GetEmployeeByInvoiceIdQuery : IRequest<Employee>
    {
        public GetEmployeeByInvoiceIdQuery(Guid id)
        {
            InvoiceId = id;
        }
        public Guid InvoiceId { get; set; }
    }
    public class GetEmployeeByInvoiceIdHandler : IRequestHandler<GetEmployeeByInvoiceIdQuery, Employee>
    {
        private readonly ITymishDbContext _context;
        public GetEmployeeByInvoiceIdHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(GetEmployeeByInvoiceIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Invoice>()
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(
                    e => e.Id == request.InvoiceId,
                    cancellationToken
                );

            if (entity.Employee == default(Employee))
            {
                throw new NotFoundException(nameof(Employee), request.InvoiceId);
            }

            return entity.Employee;
        }
    }
}