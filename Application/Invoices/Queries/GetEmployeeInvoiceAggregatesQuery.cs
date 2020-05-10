using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Tymish.Application.Dtos;
using Tymish.Application.Exceptions;

namespace Tymish.Application.Invoices.Query
{
    public class GetEmployeeInvoiceAggregatesQuery : IRequest<IList<EmployeeInvoiceAggregateDto>>
    {
        public DateTime Sent { get; set; }
    }

    public class GetEmployeeInvoiceAggregatesHandler
        : IRequestHandler<GetEmployeeInvoiceAggregatesQuery, IList<EmployeeInvoiceAggregateDto>>
    {
        private readonly ITymishDbContext _context;

        public GetEmployeeInvoiceAggregatesHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<IList<EmployeeInvoiceAggregateDto>> Handle(
            GetEmployeeInvoiceAggregatesQuery request,
            CancellationToken cancellationToken)
        {
            var aggregates = new List<EmployeeInvoiceAggregateDto>();
            var employees = await _context.Set<Employee>().ToListAsync(cancellationToken);

            foreach (var employee in employees)
            {
                var invoice = await _context
                    .Set<Invoice>()
                    .Where(e
                        => e.EmployeeId == employee.Id
                        && e.Created.Month == request.Sent.Month
                        && e.Created.Year == request.Sent.Year)
                    .SingleOrDefaultAsync(cancellationToken);

                if (invoice == default(Invoice))
                {
                    // TODO: make this exception better
                    continue;
                }

                var aggregate = new EmployeeInvoiceAggregateDto
                {
                    InvoiceId = invoice.Id,
                    Employee = employee,
                    Sent = invoice.Created,
                    Submitted = invoice.Submitted,
                    Paid = invoice.Paid
                };

                if (invoice.TimeEntries == null)
                {
                    aggregate.AmountOwed = 0m;
                }
                else
                {
                    aggregate.AmountOwed = invoice.TimeEntries
                        .Select(x => (x.End - x.Start).Hours)
                        .Sum() * employee.HourlyPay;
                }
                aggregates.Add(aggregate);
            }

            return aggregates;
        }
    }
}