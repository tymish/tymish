using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tymish.Application.Exceptions;
using Tymish.Domain.Entities;
using Tymish.Domain.Interfaces;

namespace Tymish.Application.Employees.Queries
{
    public class GetEmployeeByNumberQuery : IRequest<Employee>
    {
        public GetEmployeeByNumberQuery(int number)
        {
            EmployeeNumber = number;
        }
        public int EmployeeNumber { get; set; }
    }
    public class GetEmployeeByNumberHandler : IRequestHandler<GetEmployeeByNumberQuery, Employee>
    {
        private readonly ITymishDbContext _context;
        public GetEmployeeByNumberHandler(ITymishDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(GetEmployeeByNumberQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Employee>().SingleOrDefaultAsync(
                e => e.EmployeeNumber == request.EmployeeNumber,
                cancellationToken
            );

            if (entity == default(Employee))
            {
                throw new NotFoundException(nameof(Employee), request.EmployeeNumber);
            }

            return entity;
        }
    }
}