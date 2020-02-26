using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using System.Linq;

namespace Tymish.Application.TimeReports.Commands
{
    public class CreateTimeReportsCommand : IRequest<IEnumerable<Guid>>
    {
        public IList<int> EmployeeNumbers { get; set; }
    }

    public class CreateTimeReportsHandler : IRequestHandler<CreateTimeReportsCommand, IEnumerable<Guid>>
    {
        private readonly ITymishDbContext _context;
        private readonly IMediator _mediator;

        public CreateTimeReportsHandler(ITymishDbContext context, IMediator mediator) {
            _context = context;
            _mediator = mediator;
        }
        public async Task<IEnumerable<Guid>> Handle(CreateTimeReportsCommand request, CancellationToken cancellationToken)
        {
            // TODO: Unfortunately, EF does not allow asynchronous calls involving dbcontext
            // we must await each mediator request because each command uses the same dbcontext
            
            var timeReports = new List<TimeReport>();

            foreach (var e in request.EmployeeNumbers)
            {
                var createTimeReport = new CreateTimeReportCommand{ EmployeeNumber = e };
                var timeReport = await _mediator.Send(createTimeReport, cancellationToken);
                timeReports.Add(timeReport);
            }

            return timeReports.Select(e => e.Id);
        }
    }
}