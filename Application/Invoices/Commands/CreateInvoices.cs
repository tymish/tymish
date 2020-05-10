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
    public class CreateInvoicesCommand : IRequest<IEnumerable<Guid>>
    {
        public IList<int> EmployeeNumbers { get; set; }
    }

    public class CreateInvoicesHandler : IRequestHandler<CreateInvoicesCommand, IEnumerable<Guid>>
    {
        private readonly ITymishDbContext _context;
        private readonly IMediator _mediator;

        public CreateInvoicesHandler(ITymishDbContext context, IMediator mediator) {
            _context = context;
            _mediator = mediator;
        }
        public async Task<IEnumerable<Guid>> Handle(CreateInvoicesCommand request, CancellationToken cancellationToken)
        {
            // TODO: Unfortunately, EF does not allow asynchronous calls involving dbcontext
            // we must await each mediator request because each command uses the same dbcontext
            
            var invoices = new List<Invoice>();

            foreach (var e in request.EmployeeNumbers)
            {
                var createInvoice = new CreateInvoiceCommand{ EmployeeNumber = e };
                var invoice = await _mediator.Send(createInvoice, cancellationToken);
                invoices.Add(invoice);
            }

            return invoices.Select(e => e.Id);
        }
    }
}