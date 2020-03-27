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
    public class GetEmployeeByIdQuery : IRequest<Employee>
    {
        public GetEmployeeByIdQuery(Guid id)
        {
            EmployeeId = id;
        }
        public Guid EmployeeId { get; set; }
    }
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, Employee>
    {
        private readonly ITymishDbContext _context;
        public GetEmployeeByIdHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Employee>().SingleOrDefaultAsync(
                e => e.Id == request.EmployeeId,
                cancellationToken
            );

            if (entity == default(Employee))
            {
                throw new NotFoundException(nameof(Employee), request.EmployeeId);
            }

            return entity;
        }
    }
}