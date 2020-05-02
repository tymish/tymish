using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.Invoices.Commands
{
    public class CreateInvoiceCommand : IRequest<Invoice>
    {
        public int EmployeeNumber { get; set; }
    }

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, Invoice>
    {
        private readonly ITymishDbContext _context;

        public CreateInvoiceHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<Invoice> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            // check for not-sent time invoices before creating
            var existingInvoice = await _context.Set<Invoice>()
                .FirstOrDefaultAsync(e
                    => e.Employee.EmployeeNumber == request.EmployeeNumber 
                    && !e.Sent.HasValue
                );

            if (existingInvoice != default(Invoice))
            {
                var reason = $"time invoice without sent date still exists for employee ({request.EmployeeNumber})";
                throw new CannotCreateException(nameof(Invoice), reason);
            }

            var employee = await _context
                .Set<Employee>().SingleOrDefaultAsync(
                    e => e.EmployeeNumber == request.EmployeeNumber,
                    cancellationToken
                );
            
            if (employee == default(Employee))
            {
                throw new NotFoundException(nameof(Employee), request.EmployeeNumber);
            }

            var entity = new Invoice
            {
                Id = Guid.NewGuid(),
                Sent = null,
                Submitted = null,
                Paid = null,
                TimeEntries = null,
                Employee = employee
            };

            await _context.Set<Invoice>().AddAsync(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return entity;
        }
    }
}