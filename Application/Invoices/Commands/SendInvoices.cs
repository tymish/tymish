using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.Linq;

namespace Tymish.Application.Invoices.Commands
{
    /// <summary>Sends invoices to employees.
    /// If no invoice exists, it creates one.</summary>
    public class SendInvoicesCommand : IRequest
    {
        public DateTime Sent { get; set; }
    }

    public class SendInvoicesHandler : IRequestHandler<SendInvoicesCommand, Unit>
    {
        private readonly ITymishDbContext _context;
        private readonly IEmailGateway _email;

        public SendInvoicesHandler(
            ITymishDbContext context,
            IEmailGateway email)
        {
            _context = context;
            _email = email;
        }
        public async Task<Unit> Handle(SendInvoicesCommand request, CancellationToken cancellationToken)
        {
            var employees = await _context.Set<Employee>()
                .Include(x => x.Invoices)
                .ToListAsync(cancellationToken);

            var emailList = new List<(string, Guid)>();

            foreach(var employee in employees)
            {
                // Check if they have a invoice for this month
                var alreadyHasInvoiceForThisMonth = employee.Invoices
                    .Any(x
                    => x.Sent.HasValue
                    && x.Sent.Value.Month == request.Sent.Month
                    && x.Sent.Value.Year == request.Sent.Year
                );

                if (alreadyHasInvoiceForThisMonth)
                {
                    continue; // Skip this employee
                }
                
                var invoiceId = Guid.NewGuid();

                emailList.Add((employee.Email, invoiceId));

                await _context.Set<Invoice>().AddAsync(new Invoice()
                {
                    Id = invoiceId,
                    EmployeeId = employee.Id,
                    Sent = DateTime.UtcNow
                }, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Send email to the employees
            foreach(var email in emailList)
            {
                // send invoice message
                var toEmail = email.Item1;
                var invoiceUrl = $"https://localhost:4200/submit-invoice/{email.Item2}";
                await _email.Send(
                    toEmail,
                    "Submit your invoice for {month}",
                    invoiceUrl);
            }

            return Unit.Value;
        }
    }
}