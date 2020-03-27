using System;
using System.Threading;
using System.Threading.Tasks;
using Tymish.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using Tymish.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;

namespace Tymish.Application.TimeReports.Commands
{
    public class CreateTimeReportCommand : IRequest<TimeReport>
    {
        public int EmployeeNumber { get; set; }
    }

    public class CreateTimeReportHandler : IRequestHandler<CreateTimeReportCommand, TimeReport>
    {
        private readonly ITymishDbContext _context;

        public CreateTimeReportHandler(ITymishDbContext context) {
            _context = context;
        }
        public async Task<TimeReport> Handle(CreateTimeReportCommand request, CancellationToken cancellationToken)
        {
            // check for not-sent time reports before creating
            var existingTimeReport = await _context.Set<TimeReport>()
                .FirstOrDefaultAsync(e
                    => e.Employee.EmployeeNumber == request.EmployeeNumber 
                    && e.Sent == default(DateTime)
                );

            if (existingTimeReport != default(TimeReport))
            {
                var reason = $"time report without sent date still exists for employee ({request.EmployeeNumber})";
                throw new CannotCreateException(nameof(TimeReport), reason);
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

            var entity = new TimeReport
            {
                Id = new Guid(),
                Sent = default(DateTime),
                Submitted = default(DateTime),
                Paid = default(DateTime),
                TimeEntries = null,
                Employee = employee
            };

            await _context.Set<TimeReport>().AddAsync(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            return entity;
        }
    }
}