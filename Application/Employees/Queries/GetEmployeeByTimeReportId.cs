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
    public class GetEmployeeByTimeReportIdQuery : IRequest<Employee>
    {
        public GetEmployeeByTimeReportIdQuery(Guid id)
        {
            TimeReportId = id;
        }
        public Guid TimeReportId { get; set; }
    }
    public class GetEmployeeByTimeReportIdHandler : IRequestHandler<GetEmployeeByTimeReportIdQuery, Employee>
    {
        private readonly ITymishDbContext _context;
        public GetEmployeeByTimeReportIdHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(GetEmployeeByTimeReportIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<TimeReport>()
                .Include(e => e.Employee)
                .SingleOrDefaultAsync(
                    e => e.Id == request.TimeReportId,
                    cancellationToken
                );

            if (entity.Employee == default(Employee))
            {
                throw new NotFoundException(nameof(Employee), request.TimeReportId);
            }

            return entity.Employee;
        }
    }
}